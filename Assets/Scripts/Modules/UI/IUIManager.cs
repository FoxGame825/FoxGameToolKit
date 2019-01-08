using UnityEngine;
using UnityEngine.Events;


namespace FoxGame.UI
{
    public interface IUIManager
    {

        BaseView CurView { get; }
        Transform UIRoot { get; }


        /// <summary>
        /// 打开UI窗口
        /// </summary>
        /// <param name="viewID">窗口id</param>
        /// <param name="openTag">打开需要做的操作</param>
        /// <param name="needAnim">是否需要打开动画,该优先级大于窗口配置数据中的_ShowAnimType字段,当needAnim=false时,都不会有动画</param>
        /// <param name="onComplate">打开完成回调</param>
        /// <param name="args">传入参数</param>
        void OpenView(ViewID viewID, OpenViewTag openTag = OpenViewTag.Nune, bool needAnim = false, UnityAction onComplate = null, params object[] args);

        /// <summary>
        /// 关闭UI窗口
        /// </summary>
        /// <param name="viewID">窗口id</param>
        /// <param name="needAnim">是否需要关闭动画,同OpenView</param>
        /// <param name="needDestroy">是否完全销毁该UI,needDestroy= true时会从内存中移除,再次打开需要重新加载入内存</param>
        /// <param name="onComplate">关闭完成回调</param>
        void CloseView(ViewID viewID, bool needAnim = true, bool needDestroy = false, UnityAction onComplate = null);


        /// <summary>
        /// 导航到上一个UI, 会从导航栈中取出最近的UI打开, 仅当ViewConfigData中的_isNavigation=true时,该UI才会放入栈中
        /// </summary>
        /// <returns></returns>
        bool NavigationPreviousView();

        /// <summary>
        /// 恢复导航栈中的所有UI, 该操作会把栈中ui全部打开
        /// </summary>
        void NavigationRestore();

        /// <summary>
        /// 清空导航栈
        /// </summary>
        void NavigationClear();
    }
}
