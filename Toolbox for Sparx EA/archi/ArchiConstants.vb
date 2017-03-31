Public Class ArchiConstants
    'The name of property in foreign model for xreference purpose
    Public Const taggedValueArchiID = "ARCHI_ID"
    Public Const csvDelimiter = ","
    Public Const csvQualifier = """"
    Public Shared ReadOnly columsMappedElementsFileARCHI As String() = {"ID", "Type", "Name", "Documentation"}
    'DEPRECATED Public Const encodePropertyParameter = "%s" '%s is replaceable parameter
    'SEPRECATED Public Const encodeProperty = "{ARCHIPROPID:" + encodePropertyParameter + ":}"
    Public Const propertyEAGUID = "EAGUID"
    Public Const propertyEAID = "EAGUID"
    Public Const typeModel = "ArchimateModel"
    Public Const RelationSuffix = "Relationship"

    Public Class PropertyType
        Public Const propertyTypeOriginal = 0
        Public Const propertyTypeForeignKey = 1
    End Class
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
    Public Const typeElementTechnologyprocess = "TechnologyProcess"
    Public Const typeElementTechnologyFunction = "TechnologyFunction"
    Public Const typeElementTechnologyService = "TechnologyService"
    Public Const typeElementTechnologyArtifact = "Artifact"
    Public Const typeElementTechnologyDevice = "Device"

    'application
    Public Const typeElementAppComponent = "ApplicationComponent"
    Public Const typeElementAppFunction = "ApplicationFunction"
    Public Const typeElementAppService = "ApplicationService"
    Public Const typeElementAppInterface = "ApplicationInterface"
    Public Const typeElementAppEvent = "ApplicationEvent"

    'relation
    Public Const typeRelationRealization = "RealizationRelationship"
    Public Const typeRelationAssociation = "AssociationRelationship"
    Public Const typeRelationComposition = "CompositionRelationship"
    Public Const typeRelationServing = "ServingRelationship"
    Public Const typeRelationAssignment = "AssignmentRelationship"
    Public Const typeRelationFlow = "FlowRelationship"

End Class
