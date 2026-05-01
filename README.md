<h1 align="center">Pear 🍐 Shader</h1>

<p align="center">
  A cozy stylized Unity shader toolkit for URP and Built-In Render Pipeline with toon lighting, quick-look presets, and artist friendly controls.
</p>

<p align="center">
  <a href="https://gumoraes11.github.io/Pear-Shader/">View Website</a>
  ·
  <a href="https://github.com/GuMoraes11/Pear-Shader/releases">Download Unity Package</a>
</p>

---

## ✨ Installation

Download the latest `.unitypackage` from the Releases page:

https://github.com/GuMoraes11/Pear-Shader/releases

Then import it into Unity:

1. Open your Unity project.
2. Go to `Assets > Import Package > Custom Package`.
3. Select the latest Pear `.unitypackage`.
4. Import the `Assets/Pear` folder.

Use the correct shader for your render pipeline:

- URP: `Pear/URP/Toon`
- Built-In Render Pipeline: `Pear/BiRP/Toon`

---

## ✨ Overview

**Pear** is a cozy stylized shader toolkit for Unity. It is meant to give artists, technical artists, and non-technical users a cleaner way to control toon shading, color, outlines, and stylized material effects without digging through a messy wall of shader properties.

It started as a shader for my own Unity projects, then grew into a more focused technical art portfolio project.

Pear currently includes:

- URP toon shader: `Pear/URP/Toon`
- Built-In Render Pipeline toon shader: `Pear/BiRP/Toon`
- Custom ShaderLab/HLSL toon lighting
- Two-band and three-band toon shading
- Ramp texture support
- Static and animated hue shifting
- Rim light, emission, matcap reflection, glitter shimmer, and outline controls
- A custom Unity material inspector
- Material validation warnings and keyword sync tools
- Four quick-look presets: Pear, Cozy, Bold, and Gloss

The goal is not to become a giant all-purpose shader package. Pear is intentionally smaller, readable, and focused on clean shader code, usable editor tooling, and polished stylized results.

---

## ✨ Features

- **Toon Lighting**
  - Shadow threshold, softness, color, intensity, and ambient color controls.

- **Multi-Band Shading**
  - Optional midtone band for shadow → midtone → light workflows.

- **Ramp Texture Support**
  - Custom shading control through horizontal ramp textures.

- **Animated Hue Shift**
  - Maskable hue, saturation, brightness, and hue shift speed controls.
  - Hue shift can be static, or animated over time for color-cycling effects.

- **Pear Quick-Look Presets**
  - Pear, Cozy, Bold, and Gloss looks tuned around a cohesive pear-green, cream, and olive palette.

- **Stylized Effects**
  - Rim light, emission, matcap reflection, and glitter shimmer controls.

- **Outline Pass**
  - Inverted hull outline with width, color, and depth offset controls.
  - Note: inverted hull outlines can behave poorly on hard-edged meshes with split normals, such as Unity's default cylinder. Smooth, rounded, or custom meshes work best.

- **Custom Inspector UI**
  - Foldout groups, feature toggles, preset buttons, keyword reset, validation warnings, and a customizable accent color.

---

## ✨ Current Presets

Pear currently has four quick-look presets:

- **Pear** — a clean neutral starting material with soft toon response and a pear-green base.
- **Cozy** — a soft, warm toon look with gentle shading, subtle rim light, and a friendly handmade feel.
- **Bold** — a punchier graphic toon look with stronger banding, darker olive shadows, and a readable outline.
- **Gloss** — a sparkly pearlescent look with rim highlights and glitter shimmer for glossy toy-like surfaces.

The presets are meant to show different material directions while still feeling like they belong to the same shader family. They are simple on purpose: enough to demonstrate the system clearly, without making the project feel scattered.

---

## ✨ Demo Scene

The current demo scene is built around two rows:

- Feature examples: Outline, Rim, Shimmer, Matcap, and Hue Shift.
- Preset examples: Pear, Cozy, Bold, and Gloss.

This scene is intended for screenshots, GIFs, website images, and quick visual testing inside Unity.

---

## ✨ Project Focus

Pear is currently focused on:

- Polishing the v0.2 demo scene
- Updating screenshots and website copy
- Packaging a clean `PearShader_v0.2.0.unitypackage`
- Keeping the shader, inspector, and presets readable enough for a portfolio review

Future work may include Pear Water, ScriptableObject-based presets, stronger outline tooling, performance modes, and a more complete breakdown of the shader architecture.

---

## 📄 License

MIT License
