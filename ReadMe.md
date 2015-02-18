# BGR32ToBGR24

ディレクトリツリー内の複数のBGR32形式ビットマップをBGR24形式ビットマップに変換できるWindowsデスクトップアプリケーションです．

# Details

指定されたディレクトリとその全てのサブディレクトリを検索し，
BGR32形式でかつOpenCVで読み込み不可能なビットマップについて，BGR24形式へ変換を行います．
変換方法は別名保存と上書き保存の2つを選択できます．

* 別名保存（デフォルト）

オリジナルファイルが保存される場所の"./BGR/\*.\*"に新しく保存します．

* 上書き保存

オリジナルファイルを書き換えます．

# Requirements

* .NET Framework 4.5
* OpenCvSharp (https://github.com/shimat/opencvsharp)
* Windows API Code Pack 1.1 (https://github.com/aybe/Windows-API-Code-Pack-1.1)

本プログラムは以下のライブラリを使用しています．

* Windows API Code Pack 1.1 (c) 2010 Microsoft
* OpenCvSharp (c) 2014-2015 shimat (BSD 3-Clause License)

# Licence

The MIT License (MIT)

詳しくはLICENSE( https://github.com/dhq-boiler/BGR32ToBGR24/blob/develop/LICENSE )をご覧ください．

BGR32ToBGR24 (c) 2015 dhq_boiler.
