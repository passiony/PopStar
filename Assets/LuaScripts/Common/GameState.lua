---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by GILLAR.
--- DateTime: 2023/12/15 23:27
---

---@ class GameState 游戏状态
---@field public Load number 加载
---@field public Playing number 游戏中
---@field public Win number 胜利
---@field public Defeat number 失败
GameState = {}

GameState.Load = 0
GameState.Playing = 1
GameState.Win = 2
GameState.Defeat = 3

return GameState