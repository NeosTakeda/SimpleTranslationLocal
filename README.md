# SimpleTranslationLocal
自分用の辞書ツール

# 開発のきっかけ
[Mouse Dictionary](https://qiita.com/wtetsu/items/c43232c6c44918e977c9)は便利だと思ったものの以下を改善した方が自分的に使いやすくなるだろうと思ったため。
* Chrome以外でも使える(Windowsで使えれば十分)
* 検索結果がもう少し見やすい方が良い


# 導入
## アプリのビルド環境
* Visual Studio 16.4.5  
* .NET Framework 4.8

## 辞書データの入手
### 英辞郎
[公式サイト](https://www.eijiro.jp/index.shtml)から辞書データを購入してください

### Websters
[GitHub](https://github.com/matthewreagan/WebstersEnglishDictionary)からdictionary.jsonをダウンロードしてください。  
※ポンコツ自作パーサーを使用しているのでJSONの構文を適切に解析しているわけではありません。そのため、他のjsonを使用するとアプリが落ちるか正常に取込を行えません。

## インポート
* タスクトレイのアイコンを右クリック - Importを選択
* Selectボタンをクリックして辞書ファイルを選択
* Importボタンをクリック

自分の環境では英辞郎のファイル取込に数分かかりますので、のんびりお待ち下さい。
なお、差分取込など気の利いた処理は存在しないので、辞書単位でDELETE → INSERTを行っています。

# 検索
タスクトレイのアイコンを右クリック - Searchを選択するか Ctrl + Shift  + Alt + Lで検索ウィンドウを表示。検索したい単語を入力してEnterを押してください。１～３秒程度かかります。改善の余地はありますね。。

検索は以下の順番で行います(１で該当データなければ2を実行。２で該当データなければ３を実行）。２～３の該当データが複数存在する場合は最大５件まで表示します。目的の結果が見つからない場合は検索キーワードを変更するか、ググってください。
1. 完全一致
2. 前方一致
3. 前後方一致

# 高速検索
app.settingsファイルの UseMemoryDicitonary を手動で true にすることで高速検索が有効になります。
やっていることはDBの内容をすべてメモリに展開するだけです。検索時はメモリに展開しているリストをループ分で
ひたすら検索するだけですが、DB検索よりは遥かに早いです。

なお、アプリ起動時のメモリ展開は数十秒かかります。また、DBの情報をすべてメモリに展開するので英辞郎のデータを使用する場合は、アプリのメモリ使用量が700MBを超えます。


# ショートカットキー
Ctrl + Shift + Alt + L : 検索ウィンドウを表示 or アクティブに設定  
Ctrl + Shift + T : ウィンドウの最前面表示切り替え  
Esc : 検索ウィンドウを最小化  
Enter : 検索処理を実行
