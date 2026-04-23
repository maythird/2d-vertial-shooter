# 2D Vertical Shooter (Study)

**문서:** `main` 브랜치 기준 · 마지막 갱신 2026-04-23

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
- **파워 단계 (1~3)**: 기본값은 1이며, **파워 아이템** 픽업으로만 단계가 올라갑니다(최대 3). 단계에 따라 탄 개수·배치가 달라짐 (`CreatePower1/2/3Bullet`). *(이전에 있던 마우스 우클릭 순환 입력은 제거됨.)*
- **폭탄(Boom)**: `B` 키, 슬롯이 있을 때 소모하며 프리팹 생성 (`boomSlot`).
- **무적·리스폰**: 피격 시 짧은 **무적 시간** 동안 입력 무시·콜라이더 비활성·투명 처리 후 시작 위치로 복귀 (`TakeDamage` / `invincibleTime`). **`Player.PlayerReset()`**에서는 점수·파워와 함께 **`boomSlot`을 1로** 되돌립니다.
- **적 스폰**: `EnemyGenerator`가 랜덤 간격으로 프리팹을 스폰 포인트에서 생성. 일부 스폰은 **대각선 이동**·`Rigidbody2D` 하강 등 패턴 분기.
- **적 AI/탄**: `EnemyController`에서 이동·체력·점수·아이템 드롭 확률 처리. 이름 `"C"`인 적은 이중 탄 발사 등. 공용 적 탄 프리팹은 `EnemyBullet` 등으로 정리됨.
- **아이템**: 코인·파워·붐 슬롯 (`Item` + `type` 문자열).
- **UI**: TextMeshPro 점수, 생명·붐 아이콘 알파, 라이프 0 이하 시 **게임 오버** UI 및 오브젝트 정리 (`GameManager`). `GameScene`은 `GameManager.boomImages`가 연결되어 있고, `SampleScene`은 비어 있어 씬에 따라 붐 슬롯 UI 동작이 다를 수 있습니다.

---

## 조작 요약

| 입력 | 동작 |
|------|------|
| `Horizontal` / `Vertical` (키보드) | 플레이어 이동 |
| `Space` (홀드) | 발사 |
| *(파워 변경)* | **파워 아이템**을 먹으면 단계 상승 (`Item.cs`, 최대 3) |
| `B` | 폭탄 사용 (슬롯 소모) |

---

## 프로젝트 구조 (요약)

```
Assets/
├── Scenes/
│   ├── GameScene.unity        # 최신 플레이 테스트용 씬 (붐 UI 연결)
│   ├── SampleScene.unity      # 기본 샘플 씬
│   └── (Test.unity 제거됨)
├── Scripts/                   # 게임플레이 C# 스크립트
├── Prefabs/                   # 플레이어·적·탄·아이템·붐 등
├── Test/                      # 실험용 별도 리소스(씬/스크립트/프리팹)
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
| `Player.cs` | 이동, 발사, 파워, 폭탄, 무적, 생명·점수 필드. `PlayerReset()`에서 `score`·`power`·`boomSlot` 초기화 |
| `UIManager.cs` | 점수 TMP, 라이프/붐 UI, 게임 오버·재시작 연동 |
| `GameManager.cs` | `DontDestroyOnLoad` 싱글톤 오브젝트 관리(전역 매니저 틀) |
| `EnemyController.cs` | 적 이동·체력·피격·드롭 아이템·플레이어/탄/붐 트리거 |
| `EnemyGenerator.cs` | 적 프리팹 스폰 타이밍·방향·리지드바디 설정 |
| `Bullet.cs` / `BulletController.cs` | 플레이어·적 탄 이동·데미지·충돌 |
| `Item.cs` | 코인/파워/붐 픽업 처리 |
| `Boom.cs` | 폭탄 존재 시간 후 파괴 |
| `DrawArrow.cs` | 디버그용 화살표 표시 (스폰 방향 등) |

**태그·레이어:** 스크립트에서 `Bullets`, `EnemyBullets`, `Enemy`, `Player`, `Boom` 등 태그를 전제로 하니, 프리팹·씬 설정이 맞는지 확인하세요.

---

## 사용 패키지 (발췌)

- **2D**: Animation, PSD Importer, Sprite, Tilemap, Aseprite 등
- **Input System** (`com.unity.inputsystem`) — 프로젝트는 `activeInputHandler: 2`(Both)이며, 현재 플레이어 코드는 **레거시 `Input` API**를 사용합니다.
- **TextMesh Pro** (`com.unity.ugui` 계열)
- **Visual Scripting**, **Timeline**, **Test Framework**
- **unity-cli-connector** ([GitHub 패키지](https://github.com/youngwoocho02/unity-cli))

전체 목록은 `Packages/manifest.json`을 참고하세요.

---

## 클론 후 실행 방법

1. **Unity Hub**에서 에디터 **6000.4.1f1**을 설치합니다.
2. 이 저장소를 클론한 뒤 Hub에서 **Add**로 프로젝트 폴더를 엽니다.
3. Unity가 **`Library` 폴더**를 자동 생성합니다. (저장소에는 **용량·GitHub 제한** 때문에 `Library/`가 포함되지 않습니다.)
4. 플레이 테스트는 `Assets/Scenes/GameScene.unity`를 우선 권장합니다.
5. 기능 실험용 씬은 현재 `Assets/Test/Scenes/Test.unity`에 분리되어 있습니다.
6. 실제 빌드는 `ProjectSettings/EditorBuildSettings.asset`의 활성 씬 목록을 확인한 뒤 진행하세요. (현재 활성 목록에는 `SampleScene`만 등록)

---

## Git / 협업 참고

- 이 저장소는 **루트 `.gitignore` 화이트리스트**로, Git에 올라가는 것은 **`Assets/`**, **`Packages/`**, **`ProjectSettings/`**, **`README.md`**(및 규칙 유지용 **`.gitignore`**)뿐입니다.
- `Library/`, `Temp/`, `Logs/`, `obj/`, `UserSettings/`, `*.csproj`, `.sln`, `.idea/` 등은 **추적되지 않습니다.** 클론 후 Unity로 프로젝트를 열면 로컬에 자동 생성됩니다.

---

## 라이선스·크레딧

- **Vertical 2D Shooting BE4** 에셋 팩: 해당 폴더 및 `Readme.asset`의 안내를 따르세요.
- **Unity**, **TextMesh Pro**, 기타 Unity 패키지: 각 패키지 라이선스를 준수하세요.
- 튜토리얼 참고: **골드메탈** — 2D 종스크롤 슈팅 강의 시리즈.

---

## 알려진 이슈·메모

- 씬별 설정 차이가 있습니다. `GameScene`은 `boomImages`가 연결되어 있고, `SampleScene`은 현재 `boomImages: []`입니다.
- `EnemyGenerator`는 변수명을 `Enemies`로 정리했으며, 기존 씬/프리팹 호환을 위해 `FormerlySerializedAs("Enemys")`를 사용합니다.
- `UIManager`, `EnemyGenerator`, `BulletController` 등에 `Debug.Log`가 남아 있어 **콘솔 스팸**이 발생할 수 있습니다.
- `BulletController`는 `enum Type`(`Player` / `Enemy`)을 사용하므로, 프리팹 인스펙터의 `type` 값이 정확해야 합니다.

문의나 개선 PR은 [Issues](https://github.com/maythird/2d-vertial-shooter/issues)를 이용해 주세요.
