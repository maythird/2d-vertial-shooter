# SpaceShooter (Unity 6 URP)

**문서:** `main` 브랜치 기준 · 마지막 갱신 2026-04-28 · 웨이브 기반 적 스폰 시스템 추가

골드메탈님의 **2D 종스크롤 슈팅** 튜토리얼을 따라 만든 Unity 학습용 프로젝트입니다.  
에셋 팩 **Vertical 2D Shooting BE4**의 스프라이트·데모 리소스를 활용하고, 플레이어·적·탄·아이템·UI 로직은 `Assets/Scripts`에 구현되어 있습니다.

**원격 저장소:** [github.com/maythird/2d-vertial-shooter](https://github.com/maythird/2d-vertial-shooter)

---

## 요구 사항

| 항목 | 버전 / 비고 |
|------|-------------|
| **Unity Editor** | **6000.4.1f1** (`ProjectSettings/ProjectVersion.txt`) |
| **렌더 파이프라인** | **URP** (`com.unity.render-pipelines.universal` 17.4.0) |
| **플랫폼** | 에디터 플레이 기준 (PC). 모바일 빌드 프로필 에셋은 포함될 수 있음 |

> 다른 Unity 6 마이너 버전에서도 열릴 수 있으나, 동일 버전 사용을 권장합니다.

---

## 주요 기능

- **플레이어 이동**: 화살표 / WASD(`Horizontal`·`Vertical` 축). 화면 내 일정 영역으로 **클램프** (`Player.Clamp`).
- **연사**: `Space` 홀드 시 일정 **연사 간격**(`fireRate`)으로 탄 발사.
- **파워 단계 (1~3)**: 기본값은 1이며, **파워 아이템** 픽업으로만 단계가 올라갑니다(최대 3). 단계에 따라 탄 개수·배치가 달라짐. 파워 최대 시 파워 아이템 드롭 안 함.
- **폭탄(Boom)**: `B` 키 사용. 화면의 **모든 적과 적 탄환을 즉시 제거**하고 애니메이션 재생 후 소멸.
- **무적·리스폰**: 피격 시 짧은 **무적 시간** 동안 입력 무시·콜라이더 비활성·투명 처리 후 시작 위치로 복귀.
- **웨이브 기반 적 스폰**: `EnemyGenerator`가 `Resources/stage_data.json`을 읽어 **10웨이브** 순서대로 적을 생성. 각 항목의 `delay`(초)·`enemyType`(A/B/C)·`point`(스폰 포인트 인덱스)로 타이밍과 위치를 제어. 웨이브 사이 쿨다운(기본 3 초) 후 다음 웨이브 진행. **게임오버·일시정지 중에는 타이머 정지, 리트라이 시 1웨이브부터 리셋.**
- **아이템 드롭**: `ItemManager`가 확률·중복·파워 상한 조건을 관리. 종류별(코인·파워·붐) 최대 1개씩만 화면에 존재.
- **게임 오버**: 생명 0 → `GameManager`가 상태 전환·필드 정리·이벤트 발행 → `UIManager`가 HUD 숨김 및 게임오버 화면 표시. 재시작 버튼으로 복구.
- **오브젝트 풀링**: `PoolManager` 싱글톤이 적·탄·아이템 풀을 중앙 관리. `Instantiate`/`Destroy` 대신 `Get`/`Release`로 GC 부하 최소화.

---

## 조작 요약

| 입력 | 동작 |
|------|------|
| `Horizontal` / `Vertical` (키보드) | 플레이어 이동 |
| `Space` (홀드) | 발사 |
| `B` | 폭탄 사용 (슬롯 소모, 전체 적·적 탄 제거) |
| UI 재시작 버튼 | 게임오버 후 재시작 |

---

## 프로젝트 구조 (요약)

```
Assets/
├── Scenes/
│   ├── GameScene.unity        # 메인 플레이 씬
│   └── SampleScene.unity      # 기본 샘플 씬
├── Scripts/                   # 게임플레이 C# 스크립트
├── Prefabs/                   # 플레이어·적·탄·아이템·붐 등
├── Resources/
│   └── stage_data.json        # 웨이브별 적 스폰 데이터 (10웨이브)
├── Animation/                 # 플레이어·아이템 애니메이션
├── Settings/                  # URP 등 렌더/씬 템플릿
├── TextMesh Pro/              # TMP 리소스·폰트
└── Vertical 2D Shooting BE4/  # 스프라이트·데모·Readme 에셋 팩
Packages/
├── manifest.json
ProjectSettings/               # Unity 프로젝트 설정
```

---

## 핵심 스크립트

| 스크립트 | 역할 |
|----------|------|
| `Player.cs` | 이동·발사·파워·폭탄·무적·생명·점수. **싱글톤** (`Player.Instance`) |
| `UIManager.cs` | 점수 TMP·라이프/붐 HUD. `GameManager` 이벤트 구독으로 게임오버·재시작 UI 처리. **싱글톤** |
| `GameManager.cs` | `GameState` 관리(`Playing`/`GameOver`). `OnGameOver`·`OnRestart` 이벤트 발행. 필드 정리. **싱글톤** (`DontDestroyOnLoad`) |
| `ItemManager.cs` | 아이템 드롭 확률·중복 제한·파워 상한 체크·생성 관리. **싱글톤** |
| `PoolManager.cs` | 적·탄·아이템 오브젝트 풀 중앙 관리. Inspector에서 프리팹·예열 수 등록. **싱글톤** |
| `ObjectPool.cs` | 단일 프리팹용 풀 구현. `Get`(꺼내기)·`Release`(반환) API 제공 |
| `EnemyController.cs` | 적 이동·체력·피격·사망 시 아이템 드롭. `isDead` 플래그로 중복 드롭 방지. `OnEnable`에서 체력 초기화 |
| `EnemyGenerator.cs` | `stage_data.json`을 파싱해 웨이브 단위로 적 스폰. 코루틴(`StageRoutine` → `WaveRoutine`)으로 `delay` 대기 후 순차 생성. `GameManager.OnRestart` 구독으로 리트라이 시 1웨이브 리셋 |
| `SpwanData.cs` | `SpawnData`(적 1기 스폰 정보)·`WaveData`(웨이브)·`StageData`(전체 스테이지) 직렬화 클래스 |
| `Bullet.cs` / `BulletController.cs` | 플레이어·적 탄 이동·데미지·충돌. 풀 반환 처리 포함 |
| `Item.cs` | `ItemType` enum(Coin·Power·Boom) 기반 픽업 처리. `OnEnable`에서 이동 코루틴 시작 |
| `Boom.cs` | 사용 시 전체 Enemy·EnemyBullet 제거 후 애니메이션 재생, `duration` 후 소멸 |
| `BackGround.cs` | 배경 자동 스크롤 및 타일 재배치 루프 |
| `DrawArrow.cs` | 디버그용 화살표 표시 (스폰 방향 등) |

**태그·레이어:** `Bullets`, `EnemyBullets`, `Enemy`, `Player` 태그를 전제로 하니, 프리팹·씬 설정이 맞는지 확인하세요.

---

## 싱글톤 구조

`GameObject.Find` 없이 매니저에 접근합니다.

```
Player.Instance
GameManager.Instance   ──→ OnGameOver / OnRestart (event)
ItemManager.Instance                                  │
UIManager.Instance     ←───────────────────────────── ┤
EnemyGenerator         ←───────────────────────────── ┘ (OnRestart 구독 → 웨이브 리셋)
PoolManager.Instance   ←── EnemyController / BulletController / Item / ItemManager / EnemyGenerator
```

- `GameManager`: `DontDestroyOnLoad` 적용 (씬 전환 유지)
- `ItemManager` / `UIManager` / `Player` / `PoolManager`: 씬 종속, 중복 시 자동 제거

---

## 사용 패키지 (발췌)

- **2D**: Animation, PSD Importer, Sprite, Tilemap, Aseprite 등
- **Input System** (`com.unity.inputsystem`) — 현재 플레이어 코드는 **레거시 `Input` API**를 사용합니다.
- **TextMesh Pro** (`com.unity.ugui` 계열)
- **Visual Scripting**, **Timeline**, **Test Framework**
- **unity-cli-connector** ([GitHub 패키지](https://github.com/youngwoocho02/unity-cli))

전체 목록은 `Packages/manifest.json`을 참고하세요.

---

## 클론 후 실행 방법

1. **Unity Hub**에서 에디터 **6000.4.1f1**을 설치합니다.
2. 이 저장소를 클론한 뒤 Hub에서 **Add**로 프로젝트 폴더를 엽니다.
3. Unity가 **`Library` 폴더**를 자동 생성합니다. (저장소에는 포함되지 않습니다.)
4. 플레이 테스트는 `Assets/Scenes/GameScene.unity`를 우선 권장합니다.
5. `GameManager`, `ItemManager`, **`PoolManager`** 오브젝트가 씬에 배치되어 있어야 정상 동작합니다.
6. `PoolManager` Inspector의 **Entries**에 적·탄·아이템 프리팹을 등록하고 Warm Up 수를 설정하세요.
7. `EnemyGenerator` Inspector의 **Enemies** 배열에 EnemyA(index 0)·EnemyB(index 1)·EnemyC(index 2) 프리팹을 순서대로 등록하세요. `stage_data.json`의 `enemyType` 값과 인덱스가 일치해야 합니다.
8. 웨이브 사이 대기 시간은 `EnemyGenerator`의 **Wave Cooldown** 필드(초)에서 조정할 수 있습니다.

---

## Git / 협업 참고

- 이 저장소는 **루트 `.gitignore` 화이트리스트**로, Git에 올라가는 것은 **`Assets/`**, **`Packages/`**, **`ProjectSettings/`**, **`README.md`**(및 `.gitignore`)뿐입니다.
- `Library/`, `Temp/`, `Logs/`, `obj/`, `UserSettings/`, `*.csproj`, `.sln`, `.idea/` 등은 **추적되지 않습니다.**

---

## 라이선스·크레딧

- **Vertical 2D Shooting BE4** 에셋 팩: 해당 폴더 및 `Readme.asset`의 안내를 따르세요.
- **Unity**, **TextMesh Pro**, 기타 Unity 패키지: 각 패키지 라이선스를 준수하세요.
- 튜토리얼 참고: **골드메탈** — 2D 종스크롤 슈팅 강의 시리즈.

---

## 웨이브 데이터 구조 (`stage_data.json`)

`Assets/Resources/stage_data.json`을 수정해 스테이지 구성을 코드 없이 조정할 수 있습니다.

```json
{
  "waves": [
    {
      "enemies": [
        { "delay": 1.5, "enemyType": 0, "point": 2 },
        { "delay": 0.8, "enemyType": 1, "point": 0 }
      ]
    }
  ]
}
```

| 필드 | 타입 | 설명 |
|------|------|------|
| `delay` | `float` | 이전 적 스폰 후 대기 시간(초) |
| `enemyType` | `int` | 0 = EnemyA, 1 = EnemyB, 2 = EnemyC |
| `point` | `int` | 스폰 포인트 인덱스 (0~4: 상단 직하강 / 5~6: 좌우 대각 / 7~8: 반대 대각) |

현재 10웨이브로 구성되어 있으며, Wave 1(EnemyA 위주·느린 페이스) → Wave 10(EnemyC 집중·최단 딜레이) 순으로 난이도가 오릅니다.

---

## 알려진 이슈·메모

- `EnemyGenerator`는 변수명을 `Enemies`로 정리했으며, 기존 씬/프리팹 호환을 위해 `FormerlySerializedAs("Enemys")`를 유지합니다.
- `BulletController`는 `enum Type`(`Player`/`Enemy`)을 사용하므로, 프리팹 인스펙터의 `type` 값이 정확해야 합니다.
- `Item` 프리팹의 `itemType`은 `ItemManager`가 생성 시 직접 주입하므로, 인스펙터 설정값과 무관하게 동작합니다.
- `PoolManager`에 등록되지 않은 프리팹을 `Get`하면 런타임 오류가 발생합니다. 적·탄·아이템 프리팹을 반드시 Entries에 추가하세요.
- 플레이어 탄은 `PlayerBulletPrefab`(Power 3 중앙)과 `PlayerSmallBulletPrefab`(Power 1·2·3 측면)으로 독립 구성되어 있습니다. 두 프리팹 모두 `BulletController`와 `Bullet` 컴포넌트를 포함해야 합니다.

문의나 개선 PR은 [Issues](https://github.com/maythird/2d-vertial-shooter/issues)를 이용해 주세요.
