Module _test
    Sub ConnectorTest(ByRef m_repository As EA.Repository)

        Dim source As Object
        Dim target As Object
        Dim con As Object
        Dim o As Object

        Dim client As Object
        Dim supplier As Object

        ''use ElementIDs to quickly load an element in this example
        ''... you must find suitable IDs in your model

        'source = m_repository.GetElementByID(6061)
        'target = m_repository.GetElementByID(6008)
        source = m_repository.GetElementByID(6453)
        target = m_repository.GetElementByID(6452)

        con = source.Connectors.AddNew("test link 2", "Association")

        ''again- replace ID with a suitable one from your model
        'con.SupplierID = 6008
        con.SupplierID = 6452

        If Not con.Update Then
            Console.WriteLine(con.GetLastError)
        End If
        source.Connectors.Refresh

        Console.WriteLine("Connector Created")

        o = con.Constraints.AddNew("constraint2", "type")
        If Not o.Update Then
            Console.WriteLine(o.GetLastError)
        End If

        o = con.TaggedValues.AddNew("Tag", "Value")
        If Not o.Update Then
            Console.WriteLine(o.GetLastError)
        End If

        ''use the client and supplier ends to set
        ''additional information

        client = con.ClientEnd
        client.Visibility = "Private"
        client.Role = "m_client"
        client.Update
        supplier = con.SupplierEnd
        supplier.Visibility = "Protected"
        supplier.Role = "m_supplier"
        supplier.Update

        Console.WriteLine("Client and Supplier set")

        Console.WriteLine(client.Role)
        Console.WriteLine(supplier.Role)

    End Sub
End Module
