using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GQLTest : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        string uri = "http://127.0.0.1:24000/";
        using var graphQLClient = new GraphQLHttpClient(new Uri(uri), new NewtonsoftJsonSerializer());

        var queryRequest = new GraphQLRequest
        {
            Query = @"
              query Query {
              revision
            }",
            //OperationName = "query",
            Variables = new
            {
                buf = ""
            }
        };
        var graphQLResponse = await graphQLClient.SendQueryAsync<Revision>(queryRequest);
        if (graphQLResponse.Errors != null)
        {
            Debug.LogError(JsonConvert.SerializeObject(graphQLResponse.Errors));
        }

        Debug.Log("raw response:");
        Debug.Log(JsonConvert.SerializeObject(graphQLResponse));
        string json = JsonConvert.SerializeObject(graphQLResponse.Data);
        Debug.Log($"Data: {json}");
    }
}
