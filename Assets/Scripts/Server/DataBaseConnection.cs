using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using UnityEngine;

public class DataBaseConnection : DbConnection
{
    public override string ConnectionString { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override string Database => throw new System.NotImplementedException();

    public override string DataSource => throw new System.NotImplementedException();

    public override string ServerVersion => throw new System.NotImplementedException();

    public override ConnectionState State => throw new System.NotImplementedException();

    public override void ChangeDatabase(string databaseName)
    {
        throw new System.NotImplementedException();
    }

    public override void Close()
    {
        throw new System.NotImplementedException();
    }

    public override void Open()
    {
        throw new System.NotImplementedException();
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        throw new System.NotImplementedException();
    }

    protected override DbCommand CreateDbCommand()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        ConnectionString = "Server=myServerAddress;Database=myDatabase;User Id=myUsername;Password=myPassword;";
        Open();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
