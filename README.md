### 概要

高橋さん製作のunity-adbanner-exampleをadstir向けに改変しました。
Android Pluginです。

### 使い方(2012/11/03)

adstirさん提供のSDKを Plugins/Android/以下に置く

下記参照。<br>
第一引数：string    メディアID<br>
第二引数：int       スポットID<br>
第三引数：float     リフレッシュ間隔

```C#
    AdBannerObserver.Initialize("MEDIA-hoge", 1, 30.0);
```