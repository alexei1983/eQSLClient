# eQSL Client
Web client for uploading QSOs to eQSL.cc using AdifNet

## Usage

```
var client = new EqslClient("CallSign", "Password");
var qso = new AdifQso();
qso.SetCall("TheirCall");
qso.SetOperator("CallSign");
qso.SetDateTimeOn(DateTime.Now);
qso.SetBand("20m");
qso.SetMode("SSB", "USB");
var result = await client.Upload(qso);
```
