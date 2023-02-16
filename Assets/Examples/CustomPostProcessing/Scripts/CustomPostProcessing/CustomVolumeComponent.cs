using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pamisu.CustomPP
{

    /// <summary>
    /// 后处理插入位置
    /// </summary>
    public enum CustomPostProcessInjectionPoint
    {
        AfterOpaqueAndSky, BeforePostProcess, AfterPostProcess
    }

    /// <summary>
    /// 自定义后处理
    /// </summary>
    public abstract class CustomVolumeComponent : VolumeComponent, IPostProcessComponent, IDisposable
    {

        /// 在InjectionPoint中的渲染顺序
        public virtual int OrderInPass => 0;

        /// 插入位置
        public virtual CustomPostProcessInjectionPoint InjectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        /// 初始化，将在RenderPass加入队列时调用
        public abstract void Setup();

        /// 执行渲染
        public abstract void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination);

        #region IPostProcessComponent
        /// 返回当前组件是否处于激活状态
        public abstract bool IsActive();

        public virtual bool IsTileCompatible() => false;
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// 释放资源
        public virtual void Dispose(bool disposing) {}
        #endregion

    }

}