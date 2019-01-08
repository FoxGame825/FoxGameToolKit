# FoxGameToolKit #
unity模块化功能包,包含资源打包/加载,ui管理,消息派发... 




## 目录结构:
- Config ->配置文件夹,使用tabtoy工具将excel文件转为protobuf
- Assets/AtlasRes ->存放需要打包成SpriteAtlas的小图
- Assets/GameRes ->存放资源,ab打包文件夹


## 模块:
- AssetLoader: 资源管理模块,包含资源的同步异步加载/缓存功能.
             加载模式分为编辑器模式和ab包模式
             详细说明: http://guyuemumu.com/index.php/archives/8/

- MsgDispatcher: 消息派发模块,处理各个功能间通信,降低耦合
               详细说明:http://guyuemumu.com/index.php/archives/4/
             
- UpdateAsset: 资源更新模块,版本对比,资源下载
             详细说明: http://guyuemumu.com/index.php/archives/9/
               
- Timer: 定时器模块
- UI: UI管理模块
- ObjectPool:对象池
- Utils:其他工具
