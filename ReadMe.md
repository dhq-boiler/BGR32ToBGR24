BGR32ToBGR24 (c) 2015 dhq_boiler.

ディレクトリツリー内のたくさんのBGR32形式ビットマップをBGR24形式ビットマップに変換できます．
.NET Framework 4.5が必要です．

指定されたディレクトリとその全てのサブディレクトリを検索し，
BGR32形式でかつOpenCVで読み込み不可能なビットマップについて，BGR24形式へ変換を行います．
変換後のビットマップは変換元のビットマップファイルに対する相対パス"./BGR/*.bmp"に保存されます．

本プログラムは以下のライブラリを使用しています．

Windows API Code Pack 1.1 (c) 2010 Microsoft (MIT License)

OpenCvSharp (c) 2014-2015 shimat (BSD 3-Clause License)
