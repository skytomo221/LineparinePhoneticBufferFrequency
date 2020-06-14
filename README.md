
# リパライン語緩衝音頻度分析プログラム

リパライン語に登場する緩衝音について考察してみました。

![](https://raw.githubusercontent.com/skytomo221/LineparinePhoneticBufferFrequency/master/images/table.png)

分析の結果、上の表が作れました。
この表は、例えば、リパライン語で緩衝母音を入れずに「miss'd」にすべきか、緩衝母音を入れて「misse'd」にすべきか迷ったら、行sと列dの交わるところを見ると90%の割合で緩衝音が入っていることが分かるので、「misse'd」の方が無難ということが分かるという表です。

でも、ところどころ怪しいので注意してください。

## LineparineWordsFrequency

リパライン語コーパスを単語ごとに分解します。

## LineparineDecomposer

リパライン語の単語分解器です。
la .sozysozbot. さんの [lineparine-typing](https://github.com/sozysozbot/lineparine-typing) のソースコードを参考にしました。

## LineparinePhoneticBufferFrequency

リパライン語の緩衝音頻度分析プログラムです。
