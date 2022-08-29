Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Http
Imports Twilio.AspNet.Core.MinimalApi
Imports Twilio.TwiML
Imports Twilio.TwiML.Voice

Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)
        Dim app = builder.Build()

        app.MapGet("/what-does-the-fox-say", Function() Results.Text(
            <Response>
                <Gather action="/answer" method="GET" input="speech">
                    <Say>What does the fox say?</Say>
                </Gather>
                <Say>Ring-ding-ding-ding-dingeringeding!</Say>
            </Response>.ToString(),
            "application/xml"
            ))

        app.MapGet("/answer", Function(speechResult As String) Results.Text(
            <Response>
                <Say>You said: <%= speechResult %></Say>
            </Response>.ToString(),
            "application/xml"
            ))

        app.MapGet("/helper/what-does-the-fox-say", Function()
            Dim response = new VoiceResponse
            Dim gather = new Gather(
                input := New List(Of Gather.InputEnum) From {Voice.Gather.InputEnum.Speech},
                action := new Uri("/answer", UriKind.Absolute),
                method := Twilio.Http.HttpMethod.Get
                )
            gather.Say("What does the fox say?")
            response.Append(gather)
            response.Say("Ring-ding-ding-ding-dingeringeding!")
            Return Results.Extensions.TwiML(response)
        End Function)

        app.MapGet("/helper/answer", Function(speechResult As String) 
            Dim response = new VoiceResponse
            response.Say($"You said: {speechResult}")
            Return Results.Extensions.TwiML(response)
        End Function)
        
        app.Run()
    End Sub
End Module