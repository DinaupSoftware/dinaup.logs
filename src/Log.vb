﻿Imports System.Linq
Imports Dinaup.Logs.LogContextSchema
Imports Dinaup.LogsContextSchema
Imports Serilog.Context
Imports Serilog.Events

Public Module Log

    Public Class ElasticConfig
        Public Property Endpoint As String
        Public Property Username As String
        Public Property Password As String
        Public Property IndexPrefix As String
    End Class

    Public Class IntegrationsConfig
        Public Property MattermostWebhook As String
        Public Property TeamsWebhook As String
    End Class

    Public AutoExportContextMetric As Boolean



    Public Sub Initialize(applicationName As String,
                          applicationVersion As String,
                          elasticConfig As ElasticConfig,
                          Optional integrationsConfig As IntegrationsConfig = Nothing,
                          Optional logFilePath As String = "logs\log.txt", Optional environment As String = "Release", Optional autoExportContextMetric As Boolean = False)
        autoExportContextMetric = autoExportContextMetric
        DinaLog.Initialize(applicationName, applicationVersion, logFilePath, elasticConfig, integrationsConfig, environment)
    End Sub


    Public Async Function ElasticSearchIsAvailableAsync() As Task(Of Boolean)
        Return Await DinaLog.ElasticSearchIsAvailable
    End Function

    Public Sub SetLoggingLevel(x As LogEventLevel)
        DinaLog.LevelSwitch.MinimumLevel = x
    End Sub


    Private metricCounter_Context As New Concurrent.ConcurrentDictionary(Of String, CounterMetric)

    Private Function GetCounter(component As String, action As String) As CounterMetric
        Dim keyx = component & "-" & action
        Return metricCounter_Context.GetOrAdd(keyx, Function() New CounterMetric("context_action", "hit", 0, New Dictionary(Of String, String) From {{"component", component}, {"action", action}}))
    End Function



    Public Function IniContext(Name$, Value$) As IDisposable



        Return LogContext.PushProperty(Name, Value)
    End Function

    Public Function IniContext(Context$) As IDisposable
        Return LogContext.PushProperty("Context", Context)
    End Function

    Public Function BeginContext(component$, action$) As LogContextDisposer
        Dim componentContext As IDisposable = LogContext.PushProperty("Component", component)
        Dim actionContext As IDisposable = LogContext.PushProperty("Action", action)
        If AutoExportContextMetric Then
            Try
                Dim counter = GetCounter(component, action)
                counter.Increment()
            Catch
            End Try
        End If

        Return New LogContextDisposer(componentContext, actionContext)
    End Function
    Public Function BeginCorrelationContext(component$, action$, correlationId$) As LogContextDisposer
        Dim componentContext As IDisposable = LogContext.PushProperty("Component", component)
        Dim actionContext As IDisposable = LogContext.PushProperty("Action", action)
        Dim correlationContext As IDisposable = LogContext.PushProperty("CorrelationId", correlationId)


        If AutoExportContextMetric Then
            Try
                Dim counter = GetCounter(component, action)
                counter.Increment()
            Catch
            End Try
        End If

        Return New LogContextDisposer(componentContext, actionContext, correlationContext)
    End Function
    Public Function BeginCorrelationContextWithDetaiils(component$, action$, correlationId$, Details As Object) As LogContextDisposer
        Dim componentContext As IDisposable = LogContext.PushProperty("Component", component)
        Dim actionContext As IDisposable = LogContext.PushProperty("Action", action)
        Dim correlationContext As IDisposable = LogContext.PushProperty("CorrelationId", correlationId)
        Dim Detailsx As IDisposable = LogContext.PushProperty("Context", Details)


        If AutoExportContextMetric Then
            Try
                Dim counter = GetCounter(component, action)
                counter.Increment()
            Catch
            End Try
        End If

        Return New LogContextDisposer(componentContext, actionContext, correlationContext, Detailsx)
    End Function

    'Public Sub SendMetrics()




    'End Sub

    Public Sub CloseAndFlush()
        Serilog.Log.CloseAndFlush()
    End Sub

    Public Sub Verbose(log$)
        Serilog.Log.Verbose(log)
    End Sub
    Public Sub Debug(log$)
        Serilog.Log.Debug(log)
    End Sub
    Public Sub Information(log$)
        Serilog.Log.Information(log)
    End Sub
    Public Sub Warning(log$)
        Serilog.Log.Warning(log)
    End Sub

    Public Sub [Error](log$)
        Serilog.Log.Error(log)
    End Sub
    Public Sub Fatal(log$)
        Serilog.Log.Fatal(log)
    End Sub







    Public Sub Verbose(messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Verbose(messageTemplate, properyValues)
    End Sub
    Public Sub Debug(messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Debug(messageTemplate, properyValues)
    End Sub
    Public Sub Information(messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Information(messageTemplate, properyValues)
    End Sub
    Public Sub Warning(messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Warning(messageTemplate, properyValues)
    End Sub

    Public Sub [Error](messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Error(messageTemplate, properyValues)
    End Sub
    Public Sub Fatal(messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Fatal(messageTemplate, properyValues)
    End Sub










    Public Function ForContext(Of t)() As Serilog.ILogger
        Return Serilog.Log.ForContext(Of t)
    End Function





    Public Sub Verbose(ex As Exception, log$)
        Serilog.Log.Verbose(ex, log)
    End Sub
    Public Sub Debug(ex As Exception, log$)
        Serilog.Log.Debug(ex, log)
    End Sub
    Public Sub Information(ex As Exception, log$)
        Serilog.Log.Information(ex, log)
    End Sub
    Public Sub Warning(ex As Exception, log$)
        Serilog.Log.Warning(ex, log)
    End Sub

    Public Sub Fatal(ex As Exception, log$)
        Serilog.Log.Fatal(ex, log)
    End Sub

    Public Sub [Error](ex As Exception, log$)
        Serilog.Log.Error(ex, log)
    End Sub





    Public Sub Verbose(ex As Exception, log$, messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Verbose(ex, log, messageTemplate, properyValues)
    End Sub
    Public Sub Debug(ex As Exception, log$, messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Debug(ex, log, messageTemplate, properyValues)
    End Sub
    Public Sub Information(ex As Exception, log$, messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Information(ex, log, messageTemplate, properyValues)
    End Sub
    Public Sub Warning(ex As Exception, log$, messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Warning(ex, log, messageTemplate, properyValues)
    End Sub

    Public Sub Fatal(ex As Exception, log$, messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Fatal(ex, log, messageTemplate, properyValues)
    End Sub

    Public Sub [Error](ex As Exception, log$, messageTemplate$, ParamArray properyValues() As Object)
        Serilog.Log.Error(ex, log, messageTemplate, properyValues)
    End Sub


    Public Sub ValidateParameter(param As Guid, paramName As String)
        If param = Guid.Empty Then
            Dinaup.Logs.Error("El parámetro '{ParamName}' no puede ser empty.", paramName)
            Throw New ArgumentNullException(paramName, $"El parámetro '{paramName}' no puede ser empty.")
        End If
    End Sub


    Public Sub ValidateParameter(Of T)(param As T, paramName As String)
        If param Is Nothing Then
            Dinaup.Logs.Error("El parámetro '{ParamName}' no puede ser nulo.", paramName)
            Throw New ArgumentNullException(paramName, $"El parámetro '{paramName}' no puede ser nulo.")
        End If
    End Sub

    Public Sub ValidateParameter(param As String, paramName As String)
        If String.IsNullOrWhiteSpace(param) Then
            Dinaup.Logs.Error("El parámetro '{ParamName}' no puede ser nulo o vacío.", paramName)
            Throw New ArgumentException($"El parámetro '{paramName}' no puede ser nulo o vacío.", paramName)
        End If
    End Sub

    Public Sub ValidateParameter(param As IEnumerable, paramName As String, maxitems As Integer)


        Dim cantidad As Integer = 0
        If param IsNot Nothing Then
            For Each actual In param
                cantidad += 1
            Next
        End If

        If param Is Nothing OrElse cantidad = 0 Then
            Dinaup.Logs.Error("El parámetro '{ParamName}' no puede ser nulo o vacío.", paramName)
            Throw New ArgumentException($"El parámetro '{paramName}' no puede ser nulo o vacío.", paramName)
        ElseIf maxitems < cantidad Then
            Dinaup.Logs.Error("El parámetro '{ParamName}' no puede contener más de {maxitems} elementos.", paramName, maxitems)
            Throw New ArgumentException($"El parámetro '{paramName}' no puede contener más de {maxitems} elementos.")
        End If
    End Sub
    Public Sub ValidateParameter(param As IEnumerable, paramName As String)
        Dim cantidad As Integer = 0
        If param IsNot Nothing Then
            For Each actual In param
                cantidad += 1
            Next
        End If


        If param Is Nothing OrElse cantidad = 0 Then
            Dinaup.Logs.Error("El parámetro '{ParamName}' no puede ser nulo o vacío.", paramName)
            Throw New ArgumentException($"El parámetro '{paramName}' no puede ser nulo o vacío.", paramName)
        End If
    End Sub


End Module
