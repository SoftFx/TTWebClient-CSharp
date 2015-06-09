﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TTWebClient.Domain;
using TTWebClient.Domain.Tools;

namespace TTWebClient
{
    /// <summary>
    /// TickTrader Web Client
    /// </summary>    
    public class TickTraderWebClient
    {
        #region Private fields

        private readonly string _webApiAddress;
        private readonly string _webApiId;
        private readonly string _webApiKey;
        private readonly string _webApiSecret;
        private readonly MediaTypeFormatterCollection _formatters;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct public TickTrader Web API client
        /// </summary>
        /// <remarks>Public Web API client will access only to public methods that don't require authentication!</remarks>
        /// <param name="webApiAddress">Web API address</param>
        public TickTraderWebClient(string webApiAddress)
        {
            if (webApiAddress == null)
                throw new ArgumentNullException("webApiAddress");

            _webApiAddress = webApiAddress;

            _formatters = new MediaTypeFormatterCollection();
            _formatters.Clear();
            _formatters.Add(new JilMediaTypeFormatter());
        }

        /// <summary>
        /// Construct TickTrader Web API client
        /// </summary>
        /// <param name="webApiAddress">Web API address</param>
        /// <param name="webApiId">Web API token Id</param>
        /// <param name="webApiKey">Web API token key</param>
        /// <param name="webApiSecret">Web API token secret</param>
        public TickTraderWebClient(string webApiAddress, string webApiId, string webApiKey, string webApiSecret)
            : this(webApiAddress)
        {
            _webApiId = webApiId;
            _webApiKey = webApiKey;
            _webApiSecret = webApiSecret;
        }

        #endregion

        #region Public Web API Methods

        /// <summary>
        /// Get public trade session information
        /// </summary>
        /// <returns>Public trade session information</returns>
        public TTTradeSession GetPublicTradeSession() { return ConvertToSync(() => GetPublicTradeSessionAsync().Result); }
        public Task<TTTradeSession> GetPublicTradeSessionAsync()
        {
            return PublicHttpGetAsync<TTTradeSession>("api/v1/public/tradesession");
        }

        /// <summary>
        /// Get list of all available public currencies
        /// </summary>
        /// <returns>List of all available public currencies</returns>
        public List<TTCurrency> GetPublicAllCurrencies() { return ConvertToSync(() => GetPublicAllCurrenciesAsync().Result); }
        public Task<List<TTCurrency>> GetPublicAllCurrenciesAsync()
        {
            return PublicHttpGetAsync<List<TTCurrency>>("api/v1/public/currency");
        }

        /// <summary>
        /// Get public currency by name
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <returns>Public currency with the given name</returns>
        public TTCurrency GetPublicCurrency(string currency) { return ConvertToSync(() => GetPublicCurrencyAsync(currency).Result); }
        public Task<TTCurrency> GetPublicCurrencyAsync(string currency)
        {
            return PublicHttpGetAsync<TTCurrency>(string.Format("api/v1/public/currency/{0}", UrlEncode(currency)));
        }

        /// <summary>
        /// Get list of all available public symbols
        /// </summary>
        /// <returns>List of all available public symbols</returns>
        public List<TTSymbol> GetPublicAllSymbols() { return ConvertToSync(() => GetPublicAllSymbolsAsync().Result); }
        public Task<List<TTSymbol>> GetPublicAllSymbolsAsync()
        {
            return PublicHttpGetAsync<List<TTSymbol>>("api/v1/public/symbol");
        }

        /// <summary>
        /// Get public symbol by name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Public symbol with the given name</returns>
        public TTSymbol GetPublicSymbol(string symbol) { return ConvertToSync(() => GetPublicSymbolAsync(symbol).Result); }
        public Task<TTSymbol> GetPublicSymbolAsync(string symbol)
        {
            return PublicHttpGetAsync<TTSymbol>(string.Format("api/v1/public/symbol/{0}", UrlEncode(symbol)));
        }

        /// <summary>
        /// Get list of all available public feed ticks
        /// </summary>
        /// <returns>List of all available public feed ticks</returns>
        public List<TTFeedTick> GetPublicAllTicks() { return ConvertToSync(() => GetPublicAllTicksAsync().Result); }
        public Task<List<TTFeedTick>> GetPublicAllTicksAsync()
        {
            return PublicHttpGetAsync<List<TTFeedTick>>("api/v1/public/tick");
        }

        /// <summary>
        /// Get public feed tick by symbol name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Public feed tick with the given symbol name</returns>
        public TTFeedTick GetPublicTick(string symbol) { return ConvertToSync(() => GetPublicTickAsync(symbol).Result); }
        public Task<TTFeedTick> GetPublicTickAsync(string symbol)
        {
            return PublicHttpGetAsync<TTFeedTick>(string.Format("api/v1/public/tick/{0}", UrlEncode(symbol)));
        }

        /// <summary>
        /// Get list of all available public feed level2 ticks
        /// </summary>
        /// <returns>List of all available public feed level2 ticks</returns>
        public List<TTFeedTickLevel2> GetPublicAllTicksLevel2() { return ConvertToSync(() => GetPublicAllTicksLevel2Async().Result); }
        public Task<List<TTFeedTickLevel2>> GetPublicAllTicksLevel2Async()
        {
            return PublicHttpGetAsync<List<TTFeedTickLevel2>>("api/v1/public/level2");
        }

        /// <summary>
        /// Get public feed level2 tick by symbol name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Public feed level2 tick with the given symbol name</returns>
        public TTFeedTickLevel2 GetPublicTickLevel2(string symbol) { return ConvertToSync(() => GetPublicTickLevel2Async(symbol).Result); }
        public Task<TTFeedTickLevel2> GetPublicTickLevel2Async(string symbol)
        {
            return PublicHttpGetAsync<TTFeedTickLevel2>(string.Format("api/v1/public/level2/{0}", UrlEncode(symbol)));
        }

        #endregion

        #region Web API Methods

        /// <summary>
        /// Get account information
        /// </summary>
        /// <returns>Account information</returns>
        public TTAccount GetAccount() { return ConvertToSync(() => GetAccountAsync().Result); }
        public Task<TTAccount> GetAccountAsync()
        {
            return PrivateHttpGetAsync<TTAccount>("api/v1/account");
        }

        /// <summary>
        /// Get trade session information
        /// </summary>
        /// <returns>Trade session information</returns>
        public TTTradeSession GetTradeSession() { return ConvertToSync(() => GetTradeSessionAsync().Result); }
        public Task<TTTradeSession> GetTradeSessionAsync()
        {
            return PrivateHttpGetAsync<TTTradeSession>("api/v1/tradesession");
        }

        /// <summary>
        /// Get list of all available currencies
        /// </summary>
        /// <returns>List of all available currencies</returns>
        public List<TTCurrency> GetAllCurrencies() { return ConvertToSync(() => GetAllCurrenciesAsync().Result); }
        public Task<List<TTCurrency>> GetAllCurrenciesAsync()
        {
            return PrivateHttpGetAsync<List<TTCurrency>>("api/v1/currency");
        }

        /// <summary>
        /// Get currency by name
        /// </summary>
        /// <param name="currency">Currency name</param>
        /// <returns>Currency with the given name</returns>
        public TTCurrency GetCurrency(string currency) { return ConvertToSync(() => GetCurrencyAsync(currency).Result); }
        public Task<TTCurrency> GetCurrencyAsync(string currency)
        {
            return PrivateHttpGetAsync<TTCurrency>(string.Format("api/v1/currency/{0}", UrlEncode(currency)));
        }

        /// <summary>
        /// Get list of all available symbols
        /// </summary>
        /// <returns>List of all available symbols</returns>
        public List<TTSymbol> GetAllSymbols() { return ConvertToSync(() => GetAllSymbolsAsync().Result); }
        public Task<List<TTSymbol>> GetAllSymbolsAsync()
        {
            return PrivateHttpGetAsync<List<TTSymbol>>("api/v1/symbol");
        }

        /// <summary>
        /// Get symbol by name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Symbol with the given name</returns>
        public TTSymbol GetSymbol(string symbol) { return ConvertToSync(() => GetSymbolAsync(symbol).Result); }
        public Task<TTSymbol> GetSymbolAsync(string symbol)
        {
            return PrivateHttpGetAsync<TTSymbol>(string.Format("api/v1/symbol/{0}", UrlEncode(symbol)));
        }

        /// <summary>
        /// Get list of all available feed tick 
        /// </summary>
        /// <returns>List of all available feed tick</returns>
        public List<TTFeedTick> GetAllTicks() { return ConvertToSync(() => GetAllTicksAsync().Result); }
        public Task<List<TTFeedTick>> GetAllTicksAsync()
        {
            return PrivateHttpGetAsync<List<TTFeedTick>>("api/v1/tick");
        }

        /// <summary>
        /// Get feed tick by symbol name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Feed tick for the given symbol</returns>
        public TTFeedTick GetTick(string symbol) { return ConvertToSync(() => GetTickAsync(symbol).Result); }
        public Task<TTFeedTick> GetTickAsync(string symbol)
        {
            return PrivateHttpGetAsync<TTFeedTick>(string.Format("api/v1/tick/{0}", UrlEncode(symbol)));
        }

        /// <summary>
        /// Get list of all available feed level2 tick
        /// </summary>
        /// <returns>List of all available feed level2 tick</returns>
        public List<TTFeedTickLevel2> GetAllTicksLevel2() { return ConvertToSync(() => GetAllTicksLevel2Async().Result); }
        public Task<List<TTFeedTickLevel2>> GetAllTicksLevel2Async()
        {
            return PrivateHttpGetAsync<List<TTFeedTickLevel2>>("api/v1/level2");
        }

        /// <summary>
        /// Get feed level2 tick by symbol name
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        /// <returns>Feed level2 tick for the given symbol</returns>
        public TTFeedTickLevel2 GetTickLevel2(string symbol) { return ConvertToSync(() => GetTickLevel2Async(symbol).Result); }
        public Task<TTFeedTickLevel2> GetTickLevel2Async(string symbol)
        {
            return PrivateHttpGetAsync<TTFeedTickLevel2>(string.Format("api/v1/level2/{0}", UrlEncode(symbol)));
        }

        /// <summary>
        /// Get list of all cash account assets (currency with amount)
        /// </summary>
        /// <remarks>
        /// **Works only for cash accounts!**
        /// </remarks>>                
        /// <returns>List of all cash account assets</returns>
        public List<TTAsset> GetAllAssets() { return ConvertToSync(() => GetAllAssetsAsync().Result); }
        public Task<List<TTAsset>> GetAllAssetsAsync()
        {
            return PrivateHttpGetAsync<List<TTAsset>>("api/v1/asset");
        }

        /// <summary>
        /// Get cash account asset (currency with amount) by the given currency name
        /// </summary>
        /// <remarks>
        /// **Works only for cash accounts!**
        /// </remarks>>                        
        /// <param name="currency">Currency name</param>
        /// <returns>Cash account asset for the given currency</returns>
        public TTAsset GetAsset(string currency) { return ConvertToSync(() => GetAssetAsync(currency).Result); }
        public Task<TTAsset> GetAssetAsync(string currency)
        {
            return PrivateHttpGetAsync<TTAsset>(string.Format("api/v1/asset/{0}", UrlEncode(currency)));
        }

        /// <summary>
        /// Get list of all available positions
        /// </summary>
        /// <remarks>
        /// **Works only for net accounts!**
        /// </remarks>>                
        /// <returns>List of all available positions</returns>
        public List<TTPosition> GetAllPositions() { return ConvertToSync(() => GetAllPositionsAsync().Result); }
        public Task<List<TTPosition>> GetAllPositionsAsync()
        {
            return PrivateHttpGetAsync<List<TTPosition>>("api/v1/position");
        }

        /// <summary>
        /// Get position by symbol
        /// </summary>
        /// <remarks>
        /// **Works only for net accounts!**
        /// </remarks>>                        
        /// <param name="symbol">Symbol name</param>
        /// <returns>Position</returns>
        public TTPosition GetPosition(string symbol) { return ConvertToSync(() => GetPositionAsync(symbol).Result); }
        public Task<TTPosition> GetPositionAsync(string symbol)
        {
            return PrivateHttpGetAsync<TTPosition>(string.Format("api/v1/position/{0}", UrlEncode(symbol)));
        }

        /// <summary>
        /// Get list of all available trades
        /// </summary>
        /// <returns>List of all available trades</returns>
        public List<TTTrade> GetAllTrades() { return ConvertToSync(() => GetAllTradesAsync().Result); }
        public Task<List<TTTrade>> GetAllTradesAsync()
        {
            return PrivateHttpGetAsync<List<TTTrade>>("api/v1/trade");
        }

        /// <summary>
        /// Get trade by symbol
        /// </summary>
        /// <param name="tradeId">Trade Id</param>
        /// <returns>Trade</returns>
        public TTTrade GetTrade(long tradeId) { return ConvertToSync(() => GetTradeAsync(tradeId).Result); }
        public Task<TTTrade> GetTradeAsync(long tradeId)
        {
            return PrivateHttpGetAsync<TTTrade>(string.Format("api/v1/trade/{0}", tradeId));
        }

        /// <summary>
        /// Create new trade
        /// </summary>
        /// <remarks>
        /// New trade request is described by the filling following fields:
        /// - **ClientId** (optional) - Client trade Id
        /// - **Type** (required) - Type of trade. Possible values: `"Market"`, `"Limit"`, `"Stop"`
        /// - **Side** (required) - Side of trade. Possible values: `"Buy"`, `"Sell"`
        /// - **Symbol** (required) - Trade symbol (e.g. `"EURUSD"`)
        /// - **Price** (optional) - Price of the `"Limit"` / `"Stop"` trades (for `Market` trades price field is ignored)
        /// - **Amount** (required) - Trade amount 
        /// - **StopLoss** (optional) - Stop loss price
        /// - **TakeProfit** (optional) - Take profit price
        /// - **ExpiredTimestamp** (optional) - Expiration date and time for pending trades (`"Limit"`, `"Stop"`)
        /// - **ImmediateOrCancel** (optional) - "Immediate or cancel" flag (works only for `"Limit"` trades)
        /// - **Comment** (optional) - Client comment
        /// </remarks>        
        /// <param name="request">Create trade request</param>
        /// <returns>Created trade</returns>
        public TTTrade CreateTrade(TTTradeCreate request) { return ConvertToSync(() => CreateTradeAsync(request).Result); }
        public Task<TTTrade> CreateTradeAsync(TTTradeCreate request)
        {
            return PrivateHttpPostAsync<TTTrade, TTTradeCreate>("api/v1/trade", request);
        }

        /// <summary>
        /// Modify existing trade
        /// </summary>
        /// <remarks>
        /// Modify trade request is described by the filling following fields:
        /// - **Id** (required) - Trade Id
        /// - **Price** (optional) - New price of the `Limit` / `Stop` trades (price of `Market` trades cannot be changed)
        /// - **StopLoss** (optional) - Stop loss price
        /// - **TakeProfit** (optional) - Take profit price
        /// - **ExpiredTimestamp** (optional) - Expiration date and time for pending trades (`Limit`, `Stop`)
        /// - **Comment** (optional) - Client comment
        /// </remarks>        
        /// <param name="request">Modify trade request</param>
        /// <returns>Modified trade</returns>
        public TTTrade ModifyTrade(TTTradeModify request) { return ConvertToSync(() => ModifyTradeAsync(request).Result); }
        public Task<TTTrade> ModifyTradeAsync(TTTradeModify request)
        {
            return PrivateHttpPutAsync<TTTrade, TTTradeModify>("api/v1/trade", request);
        }

        /// <summary>
        /// Cancel existing pending trade
        /// </summary>
        /// <param name="tradeId">Trade Id to cancel</param>
        public void CancelTrade(long tradeId) { ConvertToSync(() => CancelTradeAsync(tradeId).Wait()); }
        public Task CancelTradeAsync(long tradeId)
        {
            return PrivateHttpDeleteAsync(string.Format("api/v1/trade?type=Cancel&id={0}", tradeId));
        }

        /// <summary>
        /// Close existing market trade
        /// </summary>
        /// <param name="tradeId">Trade Id to close</param>
        /// <param name="amount">Amount to close (optional)</param>
        public void CloseTrade(long tradeId, decimal? amount) { ConvertToSync(() => CloseTradeAsync(tradeId, amount).Wait()); }
        public Task CloseTradeAsync(long tradeId, decimal? amount)
        {
            return PrivateHttpDeleteAsync(amount.HasValue ? string.Format("api/v1/trade?type=Close&id={0}&amount={1}", tradeId, amount.Value) : string.Format("api/v1/trade?type=Close&id={0}", tradeId));
        }

        /// <summary>
        /// Close existing market trade by another one
        /// </summary>
        /// <param name="tradeId">Trade Id to close</param>
        /// <param name="byTradeId">By trade Id</param>
        public void CloseByTrade(long tradeId, long byTradeId) { ConvertToSync(() => CloseByTradeAsync(tradeId, byTradeId).Wait()); }
        public Task CloseByTradeAsync(long tradeId, long byTradeId)
        {
            return PrivateHttpDeleteAsync(string.Format("api/v1/trade?type=CloseBy&id={0}&byid={1}", tradeId, byTradeId));
        }

        /// <summary>
        /// Get account trade history
        /// </summary>
        /// <remarks>
        /// New trade history request is described by the filling following fields:
        /// - **TimestampFrom** (optional) - Lower timestamp bound of the trade history request
        /// - **TimestampTo** (optional) - Upper timestamp bound of the trade history request
        /// - **RequestDirection** (optional) - Request paging direction ("Forward" or "Backward"). Default is "Forward".
        /// - **RequestFromId** (optional) - Request paging from Id
        /// 
        /// If timestamps fields are not set trade history will be requests from the begin or from the current timestamp 
        /// depending on **RequestDirection** value.
        /// 
        /// Trade history is returned by chunks by paging size (default is 100). You can provide timestamp bounds (from, to)
        /// and direction of access (forward or backward). After the first request you'll get a list of trade history 
        /// records with Ids. The next request should contain **RequestFromId** with the Id of the last processed trade 
        /// history record. As the result you'll get the next chunk of trade history records. If the last page was reached 
        /// response flag **IsLastReport** will be set.
        /// </remarks>        
        /// <param name="request">Trade history request</param>
        /// <returns>Trade history report</returns>
        public TTTradeHistoryReport GetTradeHistory(TTTradeHistoryRequest request) { return ConvertToSync(() => GetTradeHistoryAsync(request).Result); }
        public Task<TTTradeHistoryReport> GetTradeHistoryAsync(TTTradeHistoryRequest request)
        {
            return PrivateHttpPostAsync<TTTradeHistoryReport, TTTradeHistoryRequest>("api/v1/tradehistory", request);
        }

        /// <summary>
        /// Get account trade history for the given trade Id
        /// </summary>
        /// <remarks>
        /// New trade history request is described by the filling following fields:
        /// - **TimestampFrom** (optional) - Lower timestamp bound of the trade history request
        /// - **TimestampTo** (optional) - Upper timestamp bound of the trade history request
        /// - **RequestDirection** (optional) - Request paging direction ("Forward" or "Backward"). Default is "Forward".
        /// - **RequestFromId** (optional) - Request paging from Id
        /// 
        /// If timestamps fields are not set trade history will be requests from the begin or from the current timestamp 
        /// depending on **RequestDirection** value.
        /// 
        /// Trade history is returned by chunks by paging size (default is 100). You can provide timestamp bounds (from, to)
        /// and direction of access (forward or backward). After the first request you'll get a list of trade history 
        /// records with Ids. The next request should contain **RequestFromId** with the Id of the last processed trade 
        /// history record. As the result you'll get the next chunk of trade history records. If the last page was reached 
        /// response flag **IsLastReport** will be set.
        /// </remarks>        
        /// <param name="tradeId">Trade Id</param>
        /// <param name="request">Trade history request</param>
        /// <returns>Trade history report</returns>
        public TTTradeHistoryReport GetTradeHistory(long tradeId, TTTradeHistoryRequest request) { return ConvertToSync(() => GetTradeHistoryAsync(tradeId, request).Result); }
        public Task<TTTradeHistoryReport> GetTradeHistoryAsync(long tradeId, TTTradeHistoryRequest request)
        {
            return PrivateHttpPostAsync<TTTradeHistoryReport, TTTradeHistoryRequest>(string.Format("api/v1/tradehistory/{0}", tradeId), request);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Force to ignore server SSL/TLS ceritficate
        /// </summary>
        public static void IgnoreServerCertificate()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;            
        }

        #endregion

        #region Private Methods

        private static string UrlEncode(string value)
        {
            return WebUtility.UrlEncode(WebUtility.UrlEncode(value));
        }

        private HttpClient CreatePublicHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_webApiAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private HttpClient CreatePrivateHttpClient()
        {
            var client = new HttpClient(new RequestContentHMACHandler(_webApiId, _webApiKey, _webApiSecret));
            client.BaseAddress = new Uri(_webApiAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private Task<TResult> PublicHttpGetAsync<TResult>(string method)
        {
            return HttpGetAsync<TResult>(CreatePublicHttpClient, method);
        }

        private Task<TResult> PrivateHttpGetAsync<TResult>(string method)
        {
            return HttpGetAsync<TResult>(CreatePrivateHttpClient, method);
        }

        private async Task<TResult> HttpGetAsync<TResult>(Func<HttpClient> clientFactory, string method)
        {
            using (var client = clientFactory())
            using (HttpResponseMessage response = await client.GetAsync(method))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TResult>(_formatters);
            }
        }

        private Task<TResult> PrivateHttpPostAsync<TResult, TRequest>(string method, TRequest request)
        {
            return HttpPostAsync<TResult, TRequest>(CreatePrivateHttpClient, method, request);
        }

        private async Task<TResult> HttpPostAsync<TResult, TRequest>(Func<HttpClient> clientFactory, string method, TRequest request)
        {
            using (var client = clientFactory())
            using (HttpResponseMessage response = await client.PostAsync(method, request, new JilMediaTypeFormatter()))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TResult>(_formatters);
            }
        }

        private Task<TResult> PrivateHttpPutAsync<TResult, TRequest>(string method, TRequest request)
        {
            return HttpPutAsync<TResult, TRequest>(CreatePrivateHttpClient, method, request);
        }

        private async Task<TResult> HttpPutAsync<TResult, TRequest>(Func<HttpClient> clientFactory, string method, TRequest request)
        {
            using (var client = clientFactory())
            using (HttpResponseMessage response = await client.PutAsync(method, request, new JilMediaTypeFormatter()))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsAsync<TResult>(_formatters);
            }
        }

        private Task PrivateHttpDeleteAsync(string method)
        {
            return HttpDeleteAsync(CreatePrivateHttpClient, method);
        }

        private async Task HttpDeleteAsync(Func<HttpClient> clientFactory, string method)
        {
            using (var client = clientFactory())
            using (HttpResponseMessage response = await client.DeleteAsync(method))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }

        private void ConvertToSync(Action method)
        {
            try
            {
                method();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.Flatten().InnerExceptions.First()).Throw();
            }
        }

        private TResult ConvertToSync<TResult>(Func<TResult> method)
        {
            try
            {
                return method();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.Flatten().InnerExceptions.First()).Throw();
                // Unreacheble code...
                return default(TResult);
            }
        }

        #endregion

        #region HMAC Client Handler

        public class RequestContentHMACHandler : HttpClientHandler
        {
            private readonly string _webApiId;
            private readonly string _webApiKey;
            private readonly string _webApiSecret;

            public RequestContentHMACHandler(string webApiId, string webApiKey, string webApiSecret)
            {
                const string message = "Account Web API methods requeies valid Web API token (Id, Key, Secret)!";
                if (string.IsNullOrEmpty(webApiId))
                    throw new ArgumentNullException("webApiId", message);
                if (string.IsNullOrEmpty(webApiKey))
                    throw new ArgumentNullException("webApiKey", message);
                if (string.IsNullOrEmpty(webApiSecret))
                    throw new ArgumentNullException("webApiSecret", message);

                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                _webApiId = webApiId;
                _webApiKey = webApiKey;
                _webApiSecret = webApiSecret;
            }
            
            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                var content = (request.Content != null) ? await request.Content.ReadAsStringAsync() : "";
                var signature = timestamp + _webApiId + _webApiKey + request.Method.Method + request.RequestUri + content;
                var hash = CalculateHmacWithSha256(signature);

                request.Headers.Authorization = new AuthenticationHeaderValue("HMAC", string.Format("{0}:{1}:{2}:{3}", _webApiId, _webApiKey, timestamp, hash));
                return await base.SendAsync(request, cancellationToken);
            }

            private string CalculateHmacWithSha256(string signature)
            {
                var encoding = new System.Text.ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(_webApiSecret);
                byte[] messageBytes = encoding.GetBytes(signature);
                using (var hmacsha256 = new HMACSHA256(keyByte))
                {
                    byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                    return Convert.ToBase64String(hashmessage);
                }
            }
        }

        #endregion
    }
}
