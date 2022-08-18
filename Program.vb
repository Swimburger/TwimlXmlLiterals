Imports System.Security
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Http

Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)
        Dim app = builder.Build()

        app.MapGet("/what-does-the-fox-say", Function() Results.Text(        
            <?xml version="1.0" encoding="utf-8"?>
            <Response>
                <Gather action="/answer" method="GET" input="speech">
                    <Say>
                        What does the fox say?
                    </Say>
                </Gather>
                <Say>Ring-ding-ding-ding-dingeringeding!</Say>
            </Response>.ToString(),
            "application/xml"
        ))

        app.MapGet("/answer", Function(speechResult As String) Results.Text(        
            <?xml version="1.0" encoding="utf-8"?>
            <Response>
                <Say>You said: <%= SecurityElement.Escape(speechResult) %></Say>
            </Response>.ToString(),
            "application/xml"
        ))

        app.Run()
    End Sub
End Module
