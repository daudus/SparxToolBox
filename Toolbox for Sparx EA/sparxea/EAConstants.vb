Public Class EAConstants
    Public Const taggedValueArchiID = "ARCHI_ID"
    Public Const metatypeArchimatePrefix = "ArchiMate3::"
    Public Const stereotypeArchimatePrefix = "ArchiMate_"

    'Archi motivation
    Public Const stereotypeElementGap = "ArchiMate_Gap"

    'business
    Public Const stereotypeElementBusinessProcess = "ArchiMate_BusinessProcess"
    Public Const stereotypeElementBusinessFunction = "ArchiMate_BusinessFunction"
    Public Const stereotypeElementBusinessService = "ArchiMate_BusinessService"
    Public Const stereotypeElementBusinessEvent = "ArchiMate_BusinessEvent"
    Public Const stereotypeElementBusinessCollaboration = "ArchiMate_BusinessCollaboration"
    Public Const stereotypeElementBusinessInteraction = "ArchiMate_BusinessInteraction"
    Public Const stereotypeElementBusinessInterface = "ArchiMate_BusinessInterface"
    Public Const stereotypeElementBusinessRole = "ArchiMate_BusinessRole"
    Public Const stereotypeElementBusinessActor = "ArchiMate_BusinessActor"
    Public Const stereotypeElementBusinessObject = "ArchiMate_BusinessObject"
    Public Const stereotypeElementContract = "ArchiMate_Contract"
    Public Const stereotypeElementRepresentation = "ArchiMate_Representation"
    Public Const stereotypeElementProduct = "ArchiMate_Product"

    'application
    Public Const stereotypeElementApplicationComponent = "ArchiMate_ApplicationComponent"
    Public Const stereotypeElementApplicationFunction = "ArchiMate_ApplicationFunction"
    Public Const stereotypeElementApplicationService = "ArchiMate_ApplicationService"
    Public Const stereotypeElementApplicationInterface = "ArchiMate_ApplicationInterface"
    Public Const stereotypeElementApplicationCollaboration = "ArchiMate_ApplicationCollaboration"
    Public Const stereotypeElementApplicationProcess = "ArchiMate_ApplicationProcess"
    Public Const stereotypeElementApplicationInteraction = "ArchiMate_ApplicationInteraction"
    Public Const stereotypeElementApplicationEvent = "ArchiMate_Applicationevent"
    Public Const stereotypeElementDataObject = "ArchiMate_DataObject"

    'technology
    Public Const stereotypeElementNode = "ArchiMate_Node"
    Public Const stereotypeElementDevice = "ArchiMate_Device"
    Public Const stereotypeElementSystemSoftware = "ArchiMate_SystemSoftware"
    Public Const stereotypeElementTechnologyCollaboration = "ArchiMate_TechnologyCollaboration"
    Public Const stereotypeElementTechnologyInterface = "ArchiMate_TechnologyInterface"
    Public Const stereotypeElementPath = "ArchiMate_Path"
    Public Const stereotypeElementCommunicationNetwork = "ArchiMate_CommunicationNetwork"
    Public Const stereotypeElementTechnologyProcess = "ArchiMate_TechnologyProcess"
    Public Const stereotypeElementTechnologyFunction = "ArchiMate_TechnologyFunction"
    Public Const stereotypeElementTechnologyInteraction = "ArchiMate_TechnologyInteraction"
    Public Const stereotypeElementTechnologyService = "ArchiMate_TechnologyService"
    Public Const stereotypeElementTechnologyEvent = "ArchiMate_TechnologyEvent"
    Public Const stereotypeElementTechnologyObject = "ArchiMate_TechnologyObject"
    Public Const stereotypeElementArtifact = "ArchiMate_Artifact"
    Public Const stereotypeElementFacility = "ArchiMate_Facility"
    Public Const stereotypeElementEquipment = "ArchiMate_Equipment"
    Public Const stereotypeElementDistributionNetwork = "ArchiMate_DistributionNetwork"
    Public Const stereotypeElementMaterial = "ArchiMate_Material"
    Public Const stereotypeElementLocation = "ArchiMate_Location"

    'relation
    Public Const stereotypeRelationComposition = "ArchiMate_Composition"
    Public Const stereotypeRelationAggregation = "ArchiMate_Aggregation"
    Public Const stereotypeRelationRealization = "ArchiMate_Realization"
    Public Const stereotypeRelationAssignment = "ArchiMate_Assignment"
    Public Const stereotypeRelationServing = "ArchiMate_Serving"
    Public Const stereotypeRelationAccess = "ArchiMate_Access"
    Public Const stereotypeRelationAssociation = "ArchiMate_Association"
    Public Const stereotypeRelationInfluence = "ArchiMate_Influence"
    Public Const stereotypeRelationTriggering = "ArchiMate_Triggering"
    Public Const stereotypeRelationFlow = "ArchiMate_Flow"
    Public Const stereotypeRelationSpecialization = "ArchiMate_Specialization"
    Public Const stereotypeRelationJunction = "ArchiMate_Junction"
    Public Const stereotypeRelationGrouping = "ArchiMate_Grouping"

    Public Const relationDirectionAny = 0
    Public Const relationDirectionClient = 1
    Public Const relationDirectionSupplier = 2

    'object type
    Public Const objectTypeActivity = "Activity"
    Public Const objectTypeClass = "Class"
    Public Const objectTypeInterface = "Interface"
    Public Const objectTypeComponent = "Component"

    'connector type
    Public Const connectorTypeAssociation = "Association"
    Public Const connectorTypeDependency = "Dependency"
    Public Const connectorTypeControlFlow = "ControlFlow"
    Public Const connectorTypeGeneralization = "Generalization"

    'connector directions
    Public Const connectorDirectionSourceDestination = "Source -> Destination"

    Public Shared ReadOnly stereotype2type As New Hashtable() From {
                                                        {stereotypeElementBusinessActor, objectTypeClass},
                                                        {stereotypeElementBusinessRole, objectTypeClass},
                                                        {stereotypeElementBusinessCollaboration, objectTypeClass},
                                                        {stereotypeElementBusinessInterface, objectTypeInterface},
                                                        {stereotypeElementBusinessInteraction, objectTypeActivity},
                                                        {stereotypeElementBusinessEvent, objectTypeActivity},
                                                        {stereotypeElementBusinessProcess, objectTypeActivity},
                                                        {stereotypeElementBusinessFunction, objectTypeActivity},
                                                        {stereotypeElementBusinessService, objectTypeActivity},
                                                        {stereotypeElementBusinessObject, objectTypeClass},
                                                        {stereotypeElementContract, objectTypeClass},
                                                        {stereotypeElementRepresentation, objectTypeClass},
                                                        {stereotypeElementProduct, objectTypeClass},
                                                        {stereotypeElementApplicationCollaboration, objectTypeClass},
                                                        {stereotypeElementApplicationComponent, objectTypeComponent},
                                                        {stereotypeElementApplicationInterface, objectTypeInterface},
                                                        {stereotypeElementApplicationProcess, objectTypeActivity},
                                                        {stereotypeElementApplicationFunction, objectTypeActivity},
                                                        {stereotypeElementApplicationInteraction, objectTypeActivity},
                                                        {stereotypeElementApplicationService, objectTypeActivity},
                                                        {stereotypeElementApplicationEvent, objectTypeActivity},
                                                        {stereotypeElementDataObject, objectTypeClass},
                                                        {stereotypeElementNode, objectTypeClass},
                                                        {stereotypeElementDevice, objectTypeClass},
                                                        {stereotypeElementSystemSoftware, objectTypeClass},
                                                        {stereotypeElementTechnologyCollaboration, objectTypeClass},
                                                        {stereotypeElementTechnologyInterface, objectTypeInterface},
                                                        {stereotypeElementPath, objectTypeClass},
                                                        {stereotypeElementCommunicationNetwork, objectTypeClass},
                                                        {stereotypeElementTechnologyProcess, objectTypeActivity},
                                                        {stereotypeElementTechnologyFunction, objectTypeActivity},
                                                        {stereotypeElementTechnologyInteraction, objectTypeActivity},
                                                        {stereotypeElementTechnologyService, objectTypeActivity},
                                                        {stereotypeElementTechnologyEvent, objectTypeActivity},
                                                        {stereotypeElementTechnologyObject, objectTypeClass},
                                                        {stereotypeElementArtifact, objectTypeClass},
                                                        {stereotypeElementFacility, objectTypeClass},
                                                        {stereotypeElementEquipment, objectTypeClass},
                                                        {stereotypeElementDistributionNetwork, objectTypeClass},
                                                        {stereotypeElementMaterial, objectTypeClass},
                                                        {stereotypeElementLocation, objectTypeClass},
                                                        {stereotypeRelationServing, connectorTypeAssociation},
                                                        {stereotypeRelationComposition, connectorTypeAssociation},
                                                        {stereotypeRelationAggregation, connectorTypeAssociation},
                                                        {stereotypeRelationRealization, connectorTypeDependency},
                                                        {stereotypeRelationAssignment, connectorTypeAssociation},
                                                        {stereotypeRelationAccess, connectorTypeDependency},
                                                        {stereotypeRelationAssociation, connectorTypeAssociation},
                                                        {stereotypeRelationInfluence, connectorTypeControlFlow},
                                                        {stereotypeRelationTriggering, connectorTypeControlFlow},
                                                        {stereotypeRelationFlow, connectorTypeControlFlow},
                                                        {stereotypeRelationSpecialization, connectorTypeGeneralization}}

    Public Shared Function typeArchi2StereotypeEA(typeArchi As String) As String
        Return stereotypeArchimatePrefix & typeArchi
    End Function

    Public Shared Function stereotypeEA2StypeEA(stereotypeEA As String) As String
        Return stereotype2type(stereotypeEA)
    End Function

End Class
