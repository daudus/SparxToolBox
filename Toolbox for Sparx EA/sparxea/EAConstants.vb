Public Class EAConstants
    Public Const activeXEA = "EA.App"
    'DEPRECATED Public Const encodePropertyParameter = "%s" '%s is replaceable parameter
    'DEPRECATED Public Const encodeProperty = "{ARCHIPROPID:" + encodePropertyParameter + ":}"
    Public Const taggedValueEAID = "~EA_ID~"
    Public Const metatypeArchimatePrefix = "ArchiMate3::"
    Public Const stereotypeArchimatePrefix = "ArchiMate_"

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
    Public Const connectorTypeBoundary = "Boundary"
    Public Const connectorTypeDecision = "Decision"

    'connector directions
    Public Const connectorDirectionSourceDestination = "Source -> Destination"
    Public Const connectorDirectionUnspecified = "Unspecified"

    Public Shared ReadOnly stereotype2type As New Hashtable() From {
            {stereotypeArchimatePrefix & Archimate3.typeOtherJunction, connectorTypeDecision},
            {stereotypeArchimatePrefix & Archimate3.typeOtherGrouping, connectorTypeBoundary},
            {stereotypeArchimatePrefix & Archimate3.typeElementPlateau, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementDeliverable, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementImplementationEvent, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementWorkPackage, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementGap, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementCourseOfAction, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementResource, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementConstraint, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementRequirement, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementPrinciple, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementOutcome, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementGoal, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementMeaning, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementValue, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementAssessment, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementStakeholder, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementDriver, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementCapability, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessActor, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessRole, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessCollaboration, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessInterface, objectTypeInterface},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessInteraction, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessEvent, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessProcess, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessFunction, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessService, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementBusinessObject, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementContract, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementRepresentation, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementProduct, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementApplicationCollaboration, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementApplicationComponent, objectTypeComponent},
            {stereotypeArchimatePrefix & Archimate3.typeElementApplicationInterface, objectTypeInterface},
            {stereotypeArchimatePrefix & Archimate3.typeElementApplicationProcess, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementApplicationFunction, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementApplicationInteraction, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementApplicationService, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementApplicationEvent, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementDataObject, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementNode, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementDevice, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementSystemSoftware, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementTechnologyCollaboration, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementTechnologyInterface, objectTypeInterface},
            {stereotypeArchimatePrefix & Archimate3.typeElementPath, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementCommunicationNetwork, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementTechnologyprocess, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementTechnologyFunction, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementTechnologyInteraction, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementTechnologyService, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementTechnologyEvent, objectTypeActivity},
            {stereotypeArchimatePrefix & Archimate3.typeElementTechnologyObject, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementArtifact, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementFacility, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementEquipment, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementDistributionNetwork, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementMaterial, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeElementLocation, objectTypeClass},
            {stereotypeArchimatePrefix & Archimate3.typeRelationServing, connectorTypeAssociation},
            {stereotypeArchimatePrefix & Archimate3.typeRelationComposition, connectorTypeAssociation},
            {stereotypeArchimatePrefix & Archimate3.typeRelationAggregation, connectorTypeAssociation},
            {stereotypeArchimatePrefix & Archimate3.typeRelationRealization, connectorTypeDependency},
            {stereotypeArchimatePrefix & Archimate3.typeRelationAssignment, connectorTypeAssociation},
            {stereotypeArchimatePrefix & Archimate3.typeRelationAccess, connectorTypeDependency},
            {stereotypeArchimatePrefix & Archimate3.typeRelationAssociation, connectorTypeAssociation},
            {stereotypeArchimatePrefix & Archimate3.typeRelationInfluence, connectorTypeControlFlow},
            {stereotypeArchimatePrefix & Archimate3.typeRelationTriggering, connectorTypeControlFlow},
            {stereotypeArchimatePrefix & Archimate3.typeRelationFlow, connectorTypeControlFlow},
            {stereotypeArchimatePrefix & Archimate3.typeRelationSpecialization, connectorTypeGeneralization}}
    'TODO:Junction is relationschips, but implemented as Elements in table t_objects. test API forproper behavior 

    Public Shared Function TypeArchi2StereotypeEA(typeArchi As String) As String
        Return stereotypeArchimatePrefix & typeArchi
    End Function

    Public Shared Function StereotypeEA2TypeEA(stereotypeEA As String) As String
        Return stereotype2type(stereotypeEA)
    End Function

End Class
