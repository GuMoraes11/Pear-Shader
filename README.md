<h1 align="center">Pear 🍐 Shader</h1>

<p align="center">
  A stylized Unity URP toon shader toolkit with presets and artist friendly controls.
</p>

<p align="center">
  <a href="https://gumoraes11.github.io/Pear-Shader/">View Website</a>
  ·
  <a href="https://github.com/GuMoraes11/Pear-Shader/releases/tag/v0.1.0">Download Unity Package</a>
</p>

---

## ✨ Installation

Download the latest `.unitypackage` from the Releases page:

https://github.com/GuMoraes11/Pear-Shader/releases

Then import it into Unity:

1. Open your Unity project.
2. Go to `Assets > Import Package > Custom Package`.
3. Select `PearShader_v0.1.0.unitypackage`.
4. Import the `Assets/Pear` folder.

Use the correct shader for your render pipeline:

- URP: `Pear/URP/Toon`
- Built-In Render Pipeline: `Pear/BiRP/Toon`

## ✨ Overview

**Pear** is a stylized shader toolkit built in Unity URP. It is meant to give artists and non-technical users a cleaner way to control toon shading, color, and stylized material effects without digging through a messy wall of shader properties.

It started as a shader for my own Unity projects, then grew into a more focused technical art portfolio project.

Pear currently includes:

- Custom ShaderLab/HLSL toon lighting
- Two-band and three-band toon shading
- Ramp texture support
- Texture hue shifting
- Rim light, emission, matcap reflection, glitter shimmer, and outline controls
- A custom Unity material inspector
- Pear-themed presets for quick art direction

The goal is not to become a giant all-purpose shader package. Pear is intentionally smaller, readable, and focused on showing clean shader code, usable editor tooling, and polished stylized results.

---

## ✨ Features

- **Toon Lighting**
  - Shadow threshold, softness, color, intensity, and ambient color controls

- **Multi-Band Shading**
  - Optional midtone band for shadow → midtone → light workflows

- **Ramp Texture Support**
  - Custom shading control through horizontal ramp textures

- **Pear-Themed Presets**
  - Soft Anime, Cel Shadow, and Glossy Toy looks tuned around a cohesive pear-green palette

- **Stylized Effects**
  - Rim light, emission, matcap reflection, and glitter shimmer controls

- **Outline Pass**
  - Inverted hull outline with width, color, and depth offset controls

- **Texture Hue Shift**
  - Maskable hue, saturation, and brightness controls for base texture variation

- **Custom Inspector UI**
  - Foldout groups, feature toggles, preset buttons, keyword reset, and a customizable accent color

---

## ✨ Current Presets

Pear currently has three focused presets:

- **Soft Anime** — gentle toon lighting, subtle rim light, soft pear/sage shadow tint, and a thin outline.
- **Cel Shadow** — hard graphic banding, olive-toned midtones and shadows, and a bolder outline.
- **Glossy Toy** — soft toy-like lighting with rim highlights and a pear shimmer.

The presets are meant to show different material directions while still feeling like they belong to the same shader. They are simple on purpose: enough to demonstrate the system clearly, without making the project feel scattered.

---

## ✨ Project Focus

Pear is currently focused on:

- Polishing the existing shader features & preset values
- Improving the demo scene
- Capturing clean screenshots and GIFs for the website
- Keeping the code and inspector readable enough for a portfolio review

Future work may include ScriptableObject-based presets, stronger demo materials, and a more complete breakdown of the shader architecture.

---

## 📄 License

MIT License
