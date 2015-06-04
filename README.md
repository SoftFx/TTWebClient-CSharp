# TTWebClient-CSharp
C# Web API client for TickTrader

## Creating Web API client
```c#
string webApiAddress = "tpdemo.fxopen.com";
string webApiId = "8bd43d1f-39a4-45cd-a876-6acc0586533d";
string webApiKey = "qXhpBKFkndWWGYQ2";
string webApiSecret = "dSccqQmtaPc2xB68GD6A7KBgpfRhHJkFe5AchGShbDGzyn8H8ThjPspCq6Yh8cTz";

// Optional: Force to ignore server certificate
TickTraderWebClient.IgnoreServerCertificate();

// Create instance of the TickTrader Web API client
var client = new TickTraderWebClient(webApiAddress, webApiId, webApiKey, webApiSecret);
```

## Access to public trade session information
```c#
// Public trade session
TTTradeSession publictradesession = client.GetPublicTradeSession().Result;
Console.WriteLine("TickTrader name: {0}", publictradesession.PlatformName);
Console.WriteLine("TickTrader company: {0}", publictradesession.PlatformCompany);
Console.WriteLine("TickTrader timezone offset: {0}", publictradesession.PlatformTimezoneOffset);
Console.WriteLine("TickTrader session status: {0}", publictradesession.SessionStatus);
```

## Access to public currencies information
```c#
// Public currency
List<TTCurrency> publicCurrencies = client.GetPublicAllCurrencies().Result;
foreach (var c in publicCurrencies)
    Console.WriteLine("Currency: " + c.Name);

TTCurrency publicCurrency = client.GetPublicCurrency(publicCurrencies[0].Name).Result;
Console.WriteLine("{0} currency precision: {1}", publicCurrency.Name, publicCurrency.Precision);
```

## Access to public symbols information
```c#
// Public symbols
List<TTSymbol> publicSymbols = client.GetPublicAllSymbols().Result;
foreach (var s in publicSymbols)
    Console.WriteLine("Symbol: " + s.Symbol);

TTSymbol publicSymbol = client.GetPublicSymbol(publicSymbols[0].Symbol).Result;
Console.WriteLine("{0} symbol precision: {1}", publicSymbol.Symbol, publicSymbol.Precision);
```

## Access to public feed ticks information
```c#
// Public feed ticks
List<TTFeedTick> publicTicks = client.GetPublicAllTicks().Result;
foreach (var t in publicTicks)
    Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

TTFeedTick publicTick = client.GetPublicTick(publicTicks[0].Symbol).Result;
Console.WriteLine("{0} tick timestamp: {1}", publicTick.Symbol, publicTick.Timestamp);
```

## Access to public feed ticks level2 information
```c#
// Public feed ticks level2
List<TTFeedTickLevel2> publicTicksLevel2 = client.GetPublicAllTicksLevel2().Result;
foreach (var t in publicTicksLevel2)
    Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

TTFeedTickLevel2 publicTickLevel2 = client.GetPublicTickLevel2(publicTicksLevel2[0].Symbol).Result;
Console.WriteLine("{0} level2 book depth: {1}", publicTickLevel2.Symbol, Math.Max(publicTickLevel2.Bids.Count, publicTickLevel2.Asks.Count));
```

## Access to account information
```c#
// Account info
TTAccount account = client.GetAccount().Result;
Console.WriteLine("Account Id: {0}", account.Id);
Console.WriteLine("Account name: {0}", account.Name);
Console.WriteLine("Account group: {0}", account.Group);
```

## Access to account trade session information
```c#
// Trade session
TTTradeSession tradesession = client.GetTradeSession().Result;
Console.WriteLine("Trade session status: {0}", tradesession.SessionStatus);
```

## Access to account currencies information
```c#
// Currencies
List<TTCurrency> currencies = client.GetAllCurrencies().Result;
foreach (var c in currencies)
    Console.WriteLine("Currency: " + c.Name);

TTCurrency currency = client.GetCurrency(currencies[0].Name).Result;
Console.WriteLine("{0} currency precision: {1}", currency.Name, currency.Precision);
```

## Access to account symbols information
```c#
// Symbols
List<TTSymbol> symbols = client.GetAllSymbols().Result;
foreach (var s in symbols)
    Console.WriteLine("Symbol: " + s.Symbol);

TTSymbol symbol = client.GetSymbol(symbols[0].Symbol).Result;
Console.WriteLine("{0} symbol precision: {1}", symbol.Symbol, symbol.Precision);
```

## Access to account feed ticks information
```c#
// Feed ticks
List<TTFeedTick> ticks = client.GetAllTicks().Result;
foreach (var t in ticks)
    Console.WriteLine("{0} tick: {1}, {2}", t.Symbol, t.BestBid.Price, t.BestAsk.Price);

TTFeedTick tick = client.GetTick(ticks[0].Symbol).Result;
Console.WriteLine("{0} tick timestamp: {1}", tick.Symbol, tick.Timestamp);
```

## Access to account feed ticks level2 information
```c#
// Feed ticks level2
List<TTFeedTickLevel2> ticksLevel2 = client.GetAllTicksLevel2().Result;
foreach (var t in ticksLevel2)
    Console.WriteLine("{0} level2 book depth: {1}", t.Symbol, Math.Max(t.Bids.Count, t.Asks.Count));

TTFeedTickLevel2 tickLevel2 = client.GetTickLevel2(ticksLevel2[0].Symbol).Result;
Console.WriteLine("{0} level2 book depth: {1}", tickLevel2.Symbol, Math.Max(tickLevel2.Bids.Count, tickLevel2.Asks.Count));
```

## Access to account assets information
Works only for cash accounts!
```c#
// Account assets
if (account.AccountingType == TTAccountingTypes.Cash)
{
    List<TTAsset> assets = client.GetAllAssets().Result;
    foreach (var a in assets)
    Console.WriteLine("{0} asset: {1}", a.Currency, a.Amount);                
}
```

## Access to account positions information
Works only for net accounts!
```c#
// Account positions
if (account.AccountingType == TTAccountingTypes.Net)
{
    List<TTPosition> positions = client.GetAllPositions().Result;
    foreach (var p in positions)
    Console.WriteLine("{0} position: {1} {2}", p.Symbol, p.LongAmount, p.ShortAmount);
}
```

## Access to account trades
```c#
// Account trades
List<TTTrade> trades = client.GetAllTrades().Result;
foreach (var t in trades)
    Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", t.Id, t.Type, t.Symbol, t.Amount);
    
// Account trade by Id
TTTrade trade = client.GetTrade(trades[0].Id).Result;
Console.WriteLine("{0} trade with type {1} by symbol {2}: {3}", t.Id, t.Type, t.Symbol, t.Amount);    
```

## Create, modify and cancel limit order
```c#
// Create, modify and cancel limit order
if ((account.AccountingType == TTAccountingTypes.Gross) || (account.AccountingType == TTAccountingTypes.Net))
{
    // Create limit order
    var limit = client.CreateTrade(new TTTradeCreate
    {
        Type = TTOrderTypes.Limit, 
        Side = TTOrderSides.Buy,
        Symbol = (account.AccountingType == TTAccountingTypes.Gross) ? "EURUSD" : "EUR/USD", 
        Amount = 10000, 
        Price = 1.0M,
        Comment = "Buy limit from Web API sample"
    }).Result;

    // Modify limit order
    limit = client.ModifyTrade(new TTTradeModify
    {
        Id = limit.Id,
        Comment = "Modified limit from Web API sample"
    }).Result;

    // Cancel limit order
    client.CancelTrade(limit.Id).Wait();
}
```

## Trade history
```c#
int iterations = 3;
var request = new TTTradeHistoryRequest { TimestampTo = DateTime.UtcNow, RequestDirection = TTStreamingDirections.Backward };

// Try to get trade history from now to the past. Request is limited to 300 records!
while (iterations-- > 0)
{
    TTTradeHistoryReport report = client.RequestTradeHistory(request).Result;
    foreach (var record in report.Records)
    {
        Console.WriteLine("TradeHistory record: Id={0}, TransactionType={1}, TransactionReason={2}, Symbol={3}, TradeId={4}", record.Id, record.TransactionType, record.TransactionReason, record.Symbol, record.TradeId);
        request.RequestLastId = record.Id;
    }
    
    // Stop for last report
    if (report.IsLastReport)
        break;
}
```
