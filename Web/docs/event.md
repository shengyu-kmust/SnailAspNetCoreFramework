## 用cap https://github.com/dotnetcore/CAP
* SnailCapConsumerServiceSelector

## 使用方法
* 类继承ICapSubscribe接口
* 在类的方法上加[CapSubscribe(EventName)]
* publisher.publish(EventName,xxx)

## 如何查看事件记录
* localhost:**/cap