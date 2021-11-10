# Unity-Reinforcement-Learning

### Path Finding with Obstacles

![](https://github.com/Shraeyas/Unity-Reinforcement-Learning/raw/main/images/MoveToGoal_Obstacles.gif)

![](https://github.com/Shraeyas/Unity-Reinforcement-Learning/raw/main/images/rewards.jpg)

| Inputs  (Continuous) | Sensors (Collected Observations) |
| -------------------- | -------------------------------- |
| moveX                | Agent Position                   |
| moveZ                | Target Position                  |
| rotateY              |                                  |

#### Child Sensors

#### Ray  Perception Sensor 3D

| Detectable  Tags | Sensor             | Value |
| ---------------- | ------------------ | ----- |
| Wall             | MaxRayDegrees      | 70    |
| Obstacle         | Sphere Cast Radius | 0.5   |

#### Rewards

| Reaches Goal | 1    |
| ------------ | ---- |
| Hit Obstacle | -1   |
| Hit Wall     | -1   |

#### Config

```
"behaviors:
  MoveToGoal:
    trainer_type: ppo
    hyperparameters:
      batch_size: 10
      buffer_size: 100
      learning_rate: 3.0e-4
      beta: 5.0e-4
      epsilon: 0.2
      lambd: 0.99
      num_epoch: 3
      learning_rate_schedule: linear
    network_settings:
      normalize: false
      hidden_units: 128
      num_layers: 2
    reward_signals:
      extrinsic:
        gamma: 0.99
        strength: 1.0
    max_steps: 500000
    time_horizon: 64
    summary_freq: 5000"
```

