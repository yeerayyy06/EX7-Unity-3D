# BaseUnityNaus3D - M0489 EX7

Videojoc de naus en 3D. Versió 3D del projecte BaseUnityNaus2D.
Unity 2022.3.46f1 · C# · TextMeshPro

---

## Com obrir el projecte a Unity

1. Obre **Unity Hub**
2. Clic a **"Open"** > **"Add project from disk"**
3. Selecciona la carpeta `BaseUnityNaus3D`
4. Unity importarà els paquets automàticament (pot trigar uns minuts)

---

## Configuració de l'escena (EscenaJoc)

### Càmera principal
- Position: `(0, 0, -15)`
- Rotation: `(0, 0, 0)`
- Projection: **Perspective** (o Orthographic size 6)

### Tags necessaris (Edit > Project Settings > Tags)
- `NauJugador`
- `Enemic`
- `ProjectilJugador`
- `ProjectilEnemic`

### Escenes (File > Build Settings > Add Open Scenes)
1. `EscenaInici`
2. `EscenaJoc`
3. `EscenaResultats`

---

## Prefabs necessaris (crear a Unity amb objectes Blender)

| Prefab | Descripció | Components |
|--------|-----------|------------|
| `NauJugador` | Cub blau (Blender) | `NauJugador`, `GeneradorProjectils`, `Rigidbody` (gravity=off), `BoxCollider` (isTrigger), Tag: NauJugador |
| `NauEnemic` | Cub vermell (Blender) | `NauEnemic`, `Rigidbody` (gravity=off), `BoxCollider` (isTrigger), Tag: Enemic |
| `NauEnemicEspecial` | Cub groc (Blender) | `NauEnemicEspecial`, `Rigidbody` (gravity=off), `BoxCollider` (isTrigger), Tag: Enemic |
| `ProjectilJugador` | Esfera verda | `ProjectilJugador`, `Rigidbody` (gravity=off), `SphereCollider` (isTrigger), Tag: ProjectilJugador |
| `ProjectilEnemic` | Esfera vermella | `ProjectilEnemic`, `Rigidbody` (gravity=off), `SphereCollider` (isTrigger), Tag: ProjectilEnemic |
| `ProjectilEnemicEspecial` | Esfera taronja | `ProjectilEnemicEspecial`, `Rigidbody` (gravity=off), `SphereCollider` (isTrigger), Tag: ProjectilEnemic |
| `ObjecteVida` | Cub verd (Blender) | `ObjecteVida`, `Rigidbody` (gravity=off), `BoxCollider` (isTrigger) |
| `Explosio` | Particle System | `Explosio` |

---

## GameObjects a l'escena EscenaJoc

### GameManager (Empty GameObject)
- Script: `GameManager`
- Assignar: BotoJugar, NauJugador, GeneradorEnemics, TextPunts, TextVides, GeneradorObjectesVida
- AudioClip: fitxer de música .mp3/.ogg (arrossegar a _musicaFons)

### NauJugador
- Posició inicial: `(0, -4, 0)`
- Scripts: `NauJugador` + `GeneradorProjectils`

### GeneradorEnemics (Empty GameObject)
- Script: `GeneradorEnemics`
- Assignar prefabs: NauEnemic, NauEnemicEspecial

### GeneradorObjectesVida (Empty GameObject)
- Script: `GeneradorObjectesVida`
- Assignar prefab: ObjecteVida

### UI (Canvas)
- **TextPunts**: TextMeshPro, script `TextPuntsJugador`, nom: "TextPunts"
- **TextVides**: TextMeshPro, script `TextVidesJugador`, nom: "TextVides"
- **BotoJugar**: Button amb OnClick → `GameManager.PassarAEstatJugant()`

---

## Enemic especial 3D (NauEnemicEspecial)

A diferència del 2D, l'enemic especial 3D té un comportament únic:
1. **Orbita** al voltant del seu punt d'aparició durant 4 segons
2. **Dispara projectils** cap al jugador des de tots els angles de l'òrbita
3. **S'acosta directament** al jugador un cop acabada l'òrbita
- Punts: 500 (vs 200 dels enemics normals)

---

## Música de fons
Afegir un fitxer `.mp3` o `.ogg` a `Assets/Audio/` i arrossegar-lo al camp `_musicaFons` del GameManager.

---

## Recerca
1. **Motors de joc**: Unity és un motor multiplataforma que permet crear jocs 2D i 3D. Usa C# com a llenguatge de scripting i ofereix un editor visual per gestionar escenes, prefabs i assets.
2. **Diferències 2D vs 3D**: En 3D s'utilitzen `Vector3`, `Rigidbody` (sense gravity), `BoxCollider`/`SphereCollider` i `OnTriggerEnter` en comptes dels equivalents 2D. La càmera necessita una posició i projecció adequades.
3. **Blender + Unity**: Blender permet crear models 3D que s'exporten com a `.fbx` o `.blend` i s'importen directament a Unity arrossegant-los a la carpeta Assets.
