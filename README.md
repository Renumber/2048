# 2048

.NET을 이용하여 만든 2048게임

### 기본 실행 화면
![2048](https://user-images.githubusercontent.com/45874237/148034295-8d3d5a95-0357-474f-a46f-aa71314cbb96.gif)

```c#
class Two{
	public static int block_num = 4;
```
Two class의 block_num값을 수정하여 기본 matrix의 크기를 바꿀 수 있습니다.(default 4x4) 
#### 5x5
![5](https://user-images.githubusercontent.com/45874237/148061604-6eedf177-a6f7-482f-868d-48aeb0220370.png)

#### 12x12
![12](https://user-images.githubusercontent.com/45874237/148061610-339da4f3-881e-474c-bd35-11f0ff8e89ac.png)


### 게임 오버
![over](https://user-images.githubusercontent.com/45874237/148058970-01dedf29-fd3c-440e-8168-c39864666d0d.png)

플레이어가 더이상 블록들을 합칠 수 없다면 게임이 종료되고, 점수가 표시됩니다.

### 랭킹
![rank](https://user-images.githubusercontent.com/45874237/148059038-ef7d3f1a-2cd1-4180-a7c3-400edee1b311.png)

게임 오버되거나 F1키를 누르면 랭킹을 확인할 수 있습니다.
