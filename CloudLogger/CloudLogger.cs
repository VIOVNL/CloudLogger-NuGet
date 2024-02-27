using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VIOVNL.CloudLogger
{
    public class CloudLoggerItem
    {
        public CloudLoggerItem(string name, object value)
        {
            Name = name;
            Value = value;
        }

        private string Name { get; set; }
        private object Value { get; set; }
    }
    public class CloudLogger
    {
        private const string HeaderName = "ProjectSecret";
        private readonly HttpClient _httpClient;
        private string _cloudLoggerUrl;
        private static CloudLogger _instance;
        private bool _throwExceptionOnFailure;

        private enum CloudLoggerResponse
        {
            Success,
            SecretFailure,
            ServerFailure
        }

    

        private CloudLogger()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Creates a singleton instance of the CloudLogger with the provided project secret and options.
        /// </summary>
        /// <param name="projectSecret">Your CloudLogger project secret. Obtain your project secret from <see href="https://cloudlogger.app">CloudLogger Website</see>.</param>
        /// <param name="throwExceptionOnFailure">Optional parameter. If set to true, the method will throw an exception on failure. Default is false.</param>
        /// <returns>The CloudLogger instance.</returns>
        /// <remarks>
        /// <para>Example:</para>
        /// <code>
        /// var cloudLogger = await CloudLogger.Create("your_project_secret", true);
        /// </code>
        /// <para>You can obtain your project secret from the CloudLogger website: <see href="https://cloudlogger.app">CloudLogger Website</see></para>
        /// </remarks>
        public static CloudLogger Create(string projectSecret, bool? throwExceptionOnFailure = null)
        {
            if (_instance == null)
            {
                _instance = new CloudLogger
                {
                    _throwExceptionOnFailure = throwExceptionOnFailure ?? false,
                    _cloudLoggerUrl = "https://api.cloudlogger.app/log"
                };
                _instance._httpClient.DefaultRequestHeaders.Add(HeaderName, projectSecret);
            }

            return _instance;
        }

        /// <summary>
        /// Updates the project secret for the CloudLogger instance, enabling logging to a different project.
        /// </summary>
        /// <param name="projectSecret">The new project secret to be set. Obtain your project secret from <see href="https://cloudlogger.app">CloudLogger Website</see>.</param>
        /// <remarks>
        /// <para>Example:</para>
        /// <code>
        /// CloudLogger.UpdateProjectSecret("your_project_secret");
        /// </code>
        /// <para>You can obtain your project secret from the CloudLogger website: <see href="https://cloudlogger.app">CloudLogger Website</see></para>
        /// </remarks>
        public void UpdateProjectSecret(string projectSecret)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(HeaderName))
            {
                _httpClient.DefaultRequestHeaders.Remove(HeaderName);
            }

            _httpClient.DefaultRequestHeaders.Add(HeaderName, projectSecret);
        }

        /// <summary>
        /// Performs the logging operation.
        /// </summary>
        /// <param name="logItems">An list of log items where each item represents a column, and the list as a whole represents a row.</param>
        /// <param name="throwExceptionOnFailure">Specifies throwing an exception in case of failure. If throwExceptionOnFailure set to true, an exception is thrown when the logging operation fails. If set to false, an error will be written in console, disregarding global ThrowExceptionOnFailure setting.</param>
        /// <remarks>
        /// <para>Basic Usage:</para>
        /// <code>
        /// await loggerSync.LogAsync([
        ///     new CloudLoggerItem("Date", "22-10-1994"),
        ///     new CloudLoggerItem("Country", "Netherlands")
        /// ]);
        /// </code>
        /// <para>With ThrowExceptionOnFailure:</para>
        /// <code>
        /// await loggerSync.LogAsync([
        ///     new CloudLoggerItem("Date", "22-10-1994"),
        ///     new CloudLoggerItem("Country", "Netherlands")
        /// ], true);
        /// </code>
        /// </remarks>
        public async Task LogAsync(List<CloudLoggerItem> logItems, bool? throwExceptionOnFailure = null)
        {
            var throwException = throwExceptionOnFailure ?? _throwExceptionOnFailure;
            try
            {
                var json = logItems.ToJson();

                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_cloudLoggerUrl}/Api/AddLog", httpContent);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (Enum.TryParse(responseBody, out CloudLoggerResponse responseEnum))
                {
                    if (!throwException)
                    {
                        return;
                    }

                    switch (responseEnum)
                    {
                        case CloudLoggerResponse.Success:
                            return;
                        case CloudLoggerResponse.SecretFailure:
                            throw new Exception("SecretFailure: CloudLogger project secret is invalid.");
                        case CloudLoggerResponse.ServerFailure:
                            throw new Exception("ServerFailure: CloudLogger server encountered an error.");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw new Exception("CloudLogger: LogAsync failed.", ex);
            }
        }
    }
}