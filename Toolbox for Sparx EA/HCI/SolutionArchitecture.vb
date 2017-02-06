Module SolutionArchitecture
    Dim Repository As EA.Repository

    Sub MainHCI(ByRef EAapp As Object)
        Dim gaps As New HashSet(Of Gap)
        Dim diagram As EA.Diagram
        Dim element As EA.Element
        Dim selectedObjectType As EA.ObjectType
        Dim enumerator As HashSet(Of Gap).Enumerator
        Dim gap As Gap

        Repository = EAapp.repository
        lLOG.Info(Repository.ConnectionString)

        Dim TEST As String = "GAP"
        Select Case TEST
            Case "APP"
                element = Repository.GetContextObject()
                selectedObjectType = Repository.GetContextItemType()
                If selectedObjectType <> EA.ObjectType.otElement Then
                    lLOG.Fatal("You have to select element!")
                    Exit Sub
                Else
                    Dim app As String
                    app = getApplicationForComponent(element)
                    lLOG.Info("For element <" + element.Name + "> is top level owning component <" + app + ">.")
                End If
            Case "GAP"
                diagram = Repository.GetContextObject()
                selectedObjectType = Repository.GetContextItemType()
                If selectedObjectType <> EA.ObjectType.otDiagram Then
                    lLOG.Fatal("You have to select diagram!")
                    Exit Sub
                End If
                lLOG.Info("Impacts for diagram: " + diagram.Name)
                getImpactsForDiagram(diagram, gaps)
                lLOG.Info(Chr(9) + getPBRForDiagram(diagram))
                printGaps(gaps)
                enumerator = gaps.GetEnumerator()
                While enumerator.MoveNext
                    gap = enumerator.Current
                    printImpactedConcepts(gap)
                End While
                'clean up
                enumerator = gaps.GetEnumerator()
                While enumerator.MoveNext
                    gap = enumerator.Current
                    gap.ImpactedConcepts.Clear()
                End While
                gaps.Clear()
                'clean up done
            Case Else
                lLOG.Error("No defined TEST!")
        End Select
        lLOG.Info("any key to exit")
    End Sub
    'simply finds for all gaps on the diagram
    Sub getImpactsForDiagram(ByRef diagram As EA.Diagram, ByRef gaps As HashSet(Of Gap))
        Dim gap As Gap
        Dim diagObj As EA.DiagramObject
        Dim element As EA.Element

        For Each diagObj In diagram.DiagramObjects
            element = Repository.GetElementByID(diagObj.ElementID)
            If (element.Stereotype = EAConstants.stereotypeElementGap) Then ' check only GAP elements
                gap = New Gap(element.ElementID, element.Name, element.Notes)
                gaps.Add(gap)
                populateElementsForGap(gap)
            End If
        Next
    End Sub
    ' for Gap finds all impacted concepts
    Sub populateElementsForGap(ByRef gap As Gap)
        Dim connector As EA.Connector
        Dim element, service, intrface, fnction, component As EA.Element
        Dim app As String = ""

        For Each connector In Repository.GetElementByID(gap.Id).Connectors
            If connector.Stereotype <> "ArchiMate_Association" Then
                lLOG.Error("Mistake against methodology. Wrong relationship: " + connector.Stereotype + " for gap " + gap.Name)
                'but do accept it
            End If
            If connector.ClientID = gap.Id Then
                element = Repository.GetElementByID(connector.SupplierID)
            Else
                element = Repository.GetElementByID(connector.ClientID)
            End If
            Select Case element.Stereotype
                Case EAConstants.stereotypeElementApplicationService
                    service = element
                    fnction = getFunctionForService(service)
                    component = getComponentForFunction(fnction)
                    app = getApplicationForComponent(component)
                    gap.ImpactedConcepts.Add(New Concept(element.Name, element.Stereotype, element.Notes, app))
                Case EAConstants.stereotypeElementApplicationFunction
                    fnction = element
                    component = getComponentForFunction(fnction)
                    app = getApplicationForComponent(component)
                    gap.ImpactedConcepts.Add(New Concept(element.Name, element.Stereotype, element.Notes, app))
                Case EAConstants.stereotypeElementApplicationInterface
                    intrface = element
                    component = getComponentForInterface(intrface)
                    app = getApplicationForComponent(component)
                    gap.ImpactedConcepts.Add(New Concept(element.Name, element.Stereotype, element.Notes, app))
                Case Else
                    lLOG.Error("Unknown stereotype " + element.Stereotype + " for concept " + element.Name)
            End Select
        Next
    End Sub

    Sub printGaps(ByRef gaps As HashSet(Of Gap))
        Dim enumerator As HashSet(Of Gap).Enumerator
        enumerator = gaps.GetEnumerator()
        Dim gap As Gap

        lLOG.Info("Number of GAPS: " + CStr(gaps.Count))
        lLOG.Info("Impact" + Chr(9) + "GAP" + Chr(9) + "Description" + Chr(9))
        While enumerator.MoveNext
            gap = enumerator.Current
            lLOG.Debug(gap.Impact + Chr(9) + gap.Name + Chr(9) + gap.Description + Chr(9))
        End While
    End Sub

    Sub printImpactedConcepts(ByRef gap As Gap)
        Dim concept As Concept
        Dim enumerator As HashSet(Of Concept).Enumerator
        enumerator = gap.ImpactedConcepts.GetEnumerator()

        lLOG.Info("Number of impacts: " + CStr(gap.ImpactedConcepts.Count))
        lLOG.Info("Application" + Chr(9) + "Concept" + Chr(9) + "Name" + Chr(9) + "Impact" + Chr(9) + "GAP")
        While enumerator.MoveNext
            concept = enumerator.Current
            lLOG.Debug(concept.Application + Chr(9) + concept.ConceptType + Chr(9) + concept.ConceptName + Chr(9) + gap.Impact + Chr(9) + gap.Name)
        End While
    End Sub

    Function getPBRForDiagram(diagram As EA.Diagram) As String
        Dim id As Integer
        Dim package As EA.Package
        Dim i As Integer
        Dim found As Boolean = False
        Dim none As Boolean = False
        Dim name As String = ""

        id = diagram.PackageID
        While (Not found) And (Not none)
            package = Repository.GetPackageByID(id)
            i = InStr(package.Name, "PBR")
            If i = 1 Then
                found = True
                name = package.Name
            Else
                id = package.ParentID
                If id = 0 Then 'package is Model
                    none = True
                End If
            End If
        End While
        Return name
    End Function
    'for given function finds the closest component - goes through all levels of functions and finds the first component
    Function getComponentForInterface(ByVal intrface As EA.Element) As EA.Element
        Dim connector As EA.Connector
        Dim countConnectors As Short
        Dim owner As EA.Element = Nothing

        connector = findRelation(intrface, EAConstants.stereotypeRelationComposition, EAConstants.relationDirectionClient)
        countConnectors = intrface.Connectors.Count
        If countConnectors = 0 Then
            Return Nothing
        End If

        owner = Repository.GetElementByID(connector.SupplierID) 'should be component
        Return owner
    End Function

    'for given Service find the closest function (the first) 
    Function getFunctionForService(ByVal service As EA.Element) As EA.Element
        Dim found As Boolean = False
        Dim connector As EA.Connector = Nothing
        Dim owner As EA.Element = Nothing
        Dim countConnectors, i As Short

        countConnectors = service.Connectors.Count

        If countConnectors = 0 Then
            Return Nothing
        End If

        While (Not found) And (i < countConnectors)
            'find proper realisation. service has to have olny one
            connector = service.Connectors(i)
            i = i + 1
            If connector.Stereotype <> EAConstants.stereotypeRelationRealization Then
                'ignore
            Else
                'it is realisation
                If connector.SupplierID = service.ElementID Then
                    'proper direction. service is Supplier - service is Realised By
                    found = True
                    owner = Repository.GetElementByID(connector.ClientID) 'should be function
                    If owner.Stereotype <> EAConstants.stereotypeElementApplicationFunction Then
                        lLOG.Error("Where is function for service " + service.Name + "?. Provided " + owner.Name + " with stereortype " + owner.Stereotype)
                    End If
                Else
                    lLOG.Error("Wrong direction for " + connector.Stereotype + " for service " + service.Name)
                End If
            End If
        End While
        If found Then
            Return owner
        Else
            Return Nothing
        End If
    End Function
    'for given function finds the closest component - goes through all levels of functions and finds the first component
    Function getComponentForFunction(ByVal fnction As EA.Element) As EA.Element
        Dim found As Boolean = False
        Dim stopp As Boolean = False
        Dim connector As EA.Connector
        Dim i, countConnectors As Short
        Dim owner As EA.Element = Nothing

        While (Not found) And (Not stopp)
            'find proper composition. fnction has to be on the Client side of such relation
            connector = findRelation(fnction, EAConstants.stereotypeRelationComposition, EAConstants.relationDirectionClient)
            If IsNothing(connector) Then 'AT the top level
                stopp = True
            Else
                fnction = Repository.GetElementByID(connector.SupplierID)
            End If
        End While

        countConnectors = fnction.Connectors.Count

        If countConnectors = 0 Then
            Return Nothing
        End If

        While (Not found) And (i < countConnectors)
            'find proper assignment. function has to have olny one
            connector = fnction.Connectors(i)
            i = i + 1
            If connector.Stereotype <> EAConstants.stereotypeRelationAssignment Then
                'ignore
            Else
                'it is assignment
                'doesn't matter on direction. 
                If connector.SupplierID = fnction.ElementID Then
                    owner = Repository.GetElementByID(connector.ClientID) 'should be component
                Else
                    owner = Repository.GetElementByID(connector.SupplierID) 'should be component
                End If
                found = True
                If owner.Stereotype <> EAConstants.stereotypeElementApplicationComponent Then
                    lLOG.Error("Where is component for sfunction " + fnction.Name + "?. Provided " + owner.Name + " with stereortype " + owner.Stereotype)
                End If
            End If
        End While
        If found Then
            Return owner
        Else
            Return Nothing
        End If
        Return fnction
    End Function
    'for given component finds the top level component aka application - goes through all levels of components and finds the top level component
    Function getApplicationForComponent(ByVal component As EA.Element) As String
        Dim found As Boolean = False
        Dim stopp As Boolean = False
        Dim connector As EA.Connector

        While (Not found) And (Not stopp)
            'find proper composition. component has to be on the Client side of such relation
            connector = findRelation(component, EAConstants.stereotypeRelationComposition, EAConstants.relationDirectionClient)
            If IsNothing(connector) Then 'AT the top level
                stopp = True
            Else
                component = Repository.GetElementByID(connector.SupplierID)
            End If
        End While
        Return component.Name
    End Function
    'simply finds the properly oriented relation of desired type from all relations belonging the component
    Function findRelation(component As EA.Element, stereotypeRelation As String, ByVal direction As Short) As EA.Connector
        Dim found As Boolean = False
        Dim connector As EA.Connector = Nothing
        Dim owner As EA.Element = Nothing
        Dim countConnectors, i As Short

        countConnectors = component.Connectors.Count

        If countConnectors = 0 Then
            Return Nothing
        End If

        While (Not found) And (i < countConnectors)
            connector = component.Connectors(i)
            i = i + 1
            If connector.Stereotype <> stereotypeRelation Then
                'ignore
            Else
                'proper relation
                Select Case direction
                    Case EAConstants.relationDirectionAny
                        found = True
                    Case EAConstants.relationDirectionClient
                        If connector.ClientID = component.ElementID Then
                            'proper direction. component is Client
                            found = True
                        End If
                    Case EAConstants.relationDirectionSupplier
                        If connector.SupplierID = component.ElementID Then
                            'proper direction. component is Supplier
                            found = True
                        End If
                    Case Else
                        lLOG.Error("Unknown desired direction of relation for component: <" + component.Name + ">")
                End Select
            End If
        End While
        If found Then
            Return connector
        Else
            Return Nothing
        End If
    End Function

End Module
