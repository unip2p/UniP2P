# UniP2P Vesion Alpha (0.2.0)
本プロジェクトはアルファ版です。

実装中の機能が多々あるのでプロダクトで使用することは推奨しません。

問題点や要望がありましたら、Issueに書き込んでいってください。

使い方やドキュメントに関しては、ReadmeとGithub Wikiを参照してください。

# 概要

UniP2PはUnityにおけるリアルタイムネットワーク通信をもっとラクに実現し、民主化しようという試みから始まったプロジェクトです。マッチング以外の処理をP2P(サーバーを使わない端末間の通信)で行うことで、低遅延かつ、サーバーに負担がかからない仕組みになっています。

MITライセンスで公開しておりマッチングサーバーに対しての同時接続数にライセンス的な制限もありません。

現在確認している対応プラットフォームは、Windows Mac Linux Androidとなっており、クロスプラットフォームでの通信が可能になっています。将来的にはWebGLでも使えるようにする予定です。

現状UDP通信に対応しています。UDPに対して独自に暗号化を組み込んでいます。
再送処理や順序制御も実装予定です。
WebGLでも使えるようにWebRTCに対応予定です。

# 機能
- オブジェクトの同期
- NetworkInstantiate
- NetworkDestroy
- RPC -Remote Procedure Call-
- ルーム作成、参加、検索
- パケットの暗号化(AES128)
- 鍵交換　(RSA PBKDF2) 
- NAT Traversal　(STUN)
- デバッグするためのエディタ拡張

# 依存ライブラリ
UniP2Pには依存ライブラリとしてUniRxとMessagePackの最新版が必要です。
またUniRxは、UniRx.Asyncも必要となります。
以下のリンクからプロジェクトに導入した上でお使いください。
- UniRx

   https://github.com/neuecc/UniRx
- MessagePack for C# 

   https://github.com/neuecc/MessagePack-CSharp
  
またUniRx.AsyncにはC#7対応が必要です。
Unity2018.3以降であればそのまま使えますが、それ以前のバージョンの場合は、IncrementalCompilerを導入してください。

# 使い方
- 1 依存ライブラリを導入してください。

- 2 [Release](https://github.com/unip2p/UniP2P/releases)からUniP2P.{version}.unitypackageをダウンロードして、Unityにインポートしてください。

- 3 マッチングサーバーを作成してその情報をMatchingSettingsに書き込む必要があります。[ここ](https://github.com/unip2p/SimpleMatchingServer-Node)を参照してください。

- 4 手軽にテストが出来るようにMatching Sample、Tanks Demo、Benchmarkを用意しています。

# 使用したライブラリ

UniRX (MIT)

https://github.com/neuecc/UniRx

https://github.com/neuecc/UniRx/blob/master/LICENSE

MessagePack for C#

https://github.com/neuecc/MessagePack-CSharp

https://github.com/neuecc/MessagePack-CSharp/blob/master/LICENSE

STUN Client (CPOL)

https://www.codeproject.com/Articles/18492/STUN-Client

https://www.codeproject.com/info/cpol10.aspx


# License
MIT License
