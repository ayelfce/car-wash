# 🚗 Car Cleaning Shader System – Unity URP

This Unity project simulates a real-time **car cleaning effect** using GPU-friendly **vertex colors** and **Shader Graph**. The car starts dirty, becomes foamy when sprayed with foam, and can only be cleaned with water **after it's been foamed**. The system dynamically calculates how clean the car is and displays it as a percentage.

---

## 🧩 Features

- ✅ Built with **Shader Graph** for Universal Render Pipeline (URP)
- ✅ Real-time **vertex color masking** (R channel-based)
- ✅ **Soft falloff brush** via SphereCast and distance-based blending
- ✅ Two spray modes: **Foam** and **Water**
- ✅ Cleans **only foamed areas**
- ✅ Real-time **foam and clean rate** analyzer
- ✅ Performance optimized – no textures or render targets needed

---

## 🎨 Shader Design

The material uses **vertex colors** to drive transitions between textures:

| `Color.r` value | Visual State |
|-----------------|---------------|
| `0.0`           | Muddy         |
| `0.5`           | Foamy         |
| `1.0`           | Clean         |

Using `Step` and `Lerp` nodes, the shader transitions between 3 texture sets:
``Mud → Foam → Clean``  
Also supports corresponding normal maps and smoothness transitions.

---

## 🛠️ Technologies Used

| Technique                 | Purpose |
|--------------------------|---------|
| `Vertex Colors`          | Dynamic masking without render textures |
| `Shader Graph` (URP)     | Texture blending & PBR control |
| `SphereCastAll`          | Multi-hit detection for soft brush radius |
| `Mathf.Clamp + Falloff`  | Smooth edge blending on brush |
| `MaskAnalyzer.cs`        | Calculates foam & clean percentages based on vertex data |

---

## ⚙️ How It Works

1. `SprayEmitter.cs` fires raycasts with a defined spray radius.
2. If the ray hits a `Car` object, it modifies the `Color.r` value of nearby vertices based on spray type:
   - Foam → raises r to `0.5`
   - Water → raises r to `1.0` **only if already ≥ 0.5**
3. `CarCleaningShader.shadergraph` blends visuals based on `r` value (mud, foam, clean).
4. `MaskAnalyzer.cs` reads all vertex colors and displays the foam & clean ratio as a percentage in the UI.

---

## 🧪 Requirements

- Unity **2022.3 LTS** or newer
- **URP** (Universal Render Pipeline) enabled
- Mesh must have `Read/Write Enabled` checked in import settings
- Mesh must contain `vertex colors`
- `Car` tag must be assigned to the vehicle for spray detection

---

## 📊 Performance Notes

- Uses **vertex data only** – no render textures or GPU readbacks
- Fully GPU-accelerated shader blending
- `Mathf.Clamp01()` ensures color values stay in valid range
- Falloff effect provides more natural spray behavior

---

## 🖼️ Demo

<img width="1172" alt="Screenshot 2025-05-14 at 22 41 23" src="https://github.com/user-attachments/assets/7aa68d5d-1180-4132-a370-1f1547422fcb" />
<img width="1169" alt="Screenshot 2025-05-14 at 22 41 52" src="https://github.com/user-attachments/assets/ec286171-dc8b-4845-b351-621b08761ef6" />


---

## ✍️ Extending the Project

This project can be adapted to simulate:
- Dirt accumulation
- Paint wear
- Scratches and damage
- Water decals or stylized visuals
