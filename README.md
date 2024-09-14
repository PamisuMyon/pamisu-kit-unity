个人的Unity游戏开发套件与一些示例项目。（开发中，当前Unity版本2022.3.x LTS）

开发套件基本成型，将会随着个人参与的项目与这里的示例项目的开发而不断更新。

示例项目龟速开发中，主要包含一些具有代表性的、功能较为常见与通用的项目，其中的功能模块可复用到具有类似需求的项目中。

# 示例项目
## [Benchmark](./samples/Benchmark/)
一些针对套件的基准测试。

## [Droid Gear](./samples/DroidGear/)（开发中）
类幸存者。

亮点：
- Character框架
- 能力系统（属性、技能、Buff)
- 模块化升级

## Tiny Farm (新建文件夹)
农场模拟。


# 开发套件
## [通用工具](./src/PamisuKit/Runtime/Common/)
包含一些基础的工具实现。

- 简易Addressable资源管理
- 有限状态机
- 对象池
- 事件总线
- 工具类（Unity、随机、数学等等）

## [Gameplay框架](./src/PamisuKit/Runtime/Framework/)
一套简单的Gameplay框架，亮点：
- 自管理的Update实现，所有事件函数有序执行且效率比原生高
- 抛弃传统单例模式，所有单例（子系统、服务等）更易于管理
- 集成事件总线，无需手动处理事件的取消订阅
