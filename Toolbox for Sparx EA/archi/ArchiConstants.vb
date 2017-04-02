Public Class ArchiConstants
    'The name of property in foreign model for xreference purpose
    Public Const taggedValueArchiID = "ARCHI_ID"
    Public Const csvDelimiter = ","
    Public Const csvQualifier = """"
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
End Class
