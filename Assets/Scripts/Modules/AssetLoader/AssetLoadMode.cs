namespace FoxGame.Asset
{
    public enum AssetLoadMode
    {
        Editor, //编辑器模式[调试运行,无需打ab包]
        AssetBundler,     //ab包模式[需要打ab包,win/ios/android平台都能使用]
    }

}
