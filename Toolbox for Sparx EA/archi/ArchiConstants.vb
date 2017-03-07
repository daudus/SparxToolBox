Public Class ArchiConstants
    Public Const csvDelimiter = ","
    Public Const csvQualifier = """"
    Public Shared ReadOnly columsMappedElementsFileARCHI As String() = {"ID", "Type", "Name", "Documentation"}
    Public Const encodePropertyParameter = "%s" '%s is replaceable parameter
    Public Const encodeProperty = "{ARCHIPROPID:" + encodePropertyParameter + ":}"
    Public Const propertyEAGUID = "EAGUID"
    Public Const propertyEAID = "EAGUID"
    Public Const typeModel = "ArchimateModel"
    Public Const RelationSuffix = "Relationship"

    'Strategy
    Public Const typeElementCapability = "Capability"

    'motivation
    Public Const typeElementGap = "Gap"

    'business
    Public Const typeElementBusProcess = "BusinessProcess"
    Public Const typeElementBusFunction = "BusinessFunction"

    'technology
    Public Const typeElementNode = "Node"
    Public Const typeElementSystemSoftware = "SystemSoftware"
    Public Const typeElementTechnologyInterface = "TechnologyInterface"
    Public Const typeElementTechnologyService = "TechnologyService"

    'application
    Public Const typeElementAppComponent = "ApplicationComponent"
    Public Const typeElementAppFunction = "ApplicationFunction"
    Public Const typeElementAppService = "ApplicationService"
    Public Const typeElementAppInterface = "ApplicationInterface"

    'relation
    Public Const typeRelationRealization = "RealizationRelationship"
    Public Const typeRelationAssociation = "AssociationRelationship"
    Public Const typeRelationComposition = "CompositionRelationship"
    Public Const typeRelationServing = "ServingRelationship"
    Public Const typeRelationAssignment = "AssignmentRelationship"
    Public Const typeRelationFlow = "FlowRelationship"

End Class
