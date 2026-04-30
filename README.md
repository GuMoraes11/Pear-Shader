<p align="center">
  <img src="docs/images/pear-hero.gif" width="600" />
</p>

<h1 align="center">🍐 Pear Shader</h1>

<p align="center">
  Stylized Unity URP toon shader toolkit with user-friendly controls.
</p>

---

<p align="center">
  <img src="https://img.shields.io/badge/Unity-URP-000000?style=for-the-badge&logo=unity" />
  <img src="https://img.shields.io/badge/Shader-HLSL-green?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Status-In%20Development-9bd744?style=for-the-badge" />
  <img src="https://img.shields.io/badge/License-MIT-blue?style=for-the-badge" />
</p>

---

## ✨ Overview

**Pear** is a stylized shader toolkit built in Unity URP, designed to give artists clean, intuitive control over toon shading and stylized rendering.

It combines:

- Custom ShaderLab/HLSL lighting
- Multi-band toon shading
- Ramp-based shading workflows
- A fully custom Unity material inspector
- Stylized effects like rim light, emission, and matcap

Built as a **technical art portfolio project**, Pear focuses on readability, usability, and clean architecture.

---

## 🎯 Features

- 🎨 **Toon Lighting**
  - Shadow threshold, softness, color, intensity

- 🌗 **Multi-Band Shading**
  - Optional midtone band (shadow → midtone → light)

- 🌈 **Ramp Texture Support**
  - Custom shading control via textures

- ✨ **Rim Lighting**
  - Fresnel-based stylized highlights

- 💡 **Emission**
  - Color, mask, intensity, pulse

- 🧊 **Matcap Reflection**
  - Fake glossy / plastic / pearlescent look

- 🧰 **Custom Inspector UI**
  - Foldout groups
  - Feature toggles
  - Clean artist workflow

---

## 📦 Installation

1. Clone or download this repository
2. Open your Unity URP project
3. Copy the `Assets/Pear` folder into your project

---

## 🧪 Demo

<p align="center">
  <img src="docs/images/pear-comparison.png" width="700"/>
</p>

---

## 🛠️ Core Files

- `PearToon.shader` — main shader logic
- `PearToonShaderGUI.cs` — custom material inspector

---

## 🚧 Roadmap

- [x] Base toon lighting
- [x] Multi-band shading
- [x] Ramp texture support
- [x] Rim lighting
- [x] Emission
- [x] Matcap
- [x] Custom inspector UI
- [ ] Inverted hull outline
- [ ] Preset system
- [ ] Demo scene polish
- [ ] Documentation pass

---

## 📚 Technical Focus

This project demonstrates:

- URP shader development (HLSL)
- Custom lighting models
- Shader keyword systems
- Unity Editor tooling (ShaderGUI)
- Artist-focused UX design

---

## 📄 License

MIT License
