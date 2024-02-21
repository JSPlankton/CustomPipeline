using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LiteRP
{
    public class LiteRenderPipeline : RenderPipeline
    {
        //老版本 兼容性
        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {

        }

        //新版本
        protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
        {
            //开始渲染上下文 -- RenderPipelineManager管理每帧开始前/结束后的注册事件回调
            BeginContextRendering(context, cameras);
            
            //渲染相机 Scene Cam, Preview Cam, Game Cam...
            for (int i = 0; i < cameras.Count; i++)
            {
                Camera camera = cameras[i];
                RenderCamera(context, camera);
            }
            
            //结束渲染上下文
            EndContextRendering(context, cameras);
        }

        private void RenderCamera(ScriptableRenderContext context, Camera camera)
        {
            //开始渲染相机 -- RenderPipelineManager管理每帧相机渲染开始前/结束后的注册事件回调
            BeginCameraRendering(context, camera);
            
            //---相机渲染流程 - 引擎固定流程 - begin -
            
            // 1.获取相机剔除参数，并进行剔除操作 - Profiler = CullScriptable
            ScriptableCullingParameters cullingParameters;
            if (!camera.TryGetCullingParameters(out cullingParameters))
                return;

            CullingResults cullingResults = context.Cull(ref cullingParameters);
            
            // 2.为相机创建CommandBuff - 所有渲染的操作都是通过命令的方式来完成
            // CommandBufferPool - 依赖 Assmbly core, core.shared
            CommandBuffer cmd = CommandBufferPool.Get(camera.name);
            // 3.设置相机属性参数
            context.SetupCameraProperties(camera);
            // 4.清理渲染目标
            // 5.指定渲染排序设置 SortSettings
            // 6.指定渲染状态设置 DrawSettings
            // 7.指定渲染过滤设置 FilterSettings
            // 8.创建渲染列表
            // 9.绘制渲染列表
            // 10.提交命令缓冲区
            context.ExecuteCommandBuffer(cmd);
            // 11.释放命令缓冲区
            cmd.Clear();
            CommandBufferPool.Release(cmd);
            // 12.提交渲染上下文
            context.Submit();
            
            //相机渲染流程 - 引擎固定流程 - end ---
            
            //结束渲染相机
            EndCameraRendering(context, camera);
        }


    }
}