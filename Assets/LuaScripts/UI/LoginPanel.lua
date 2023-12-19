---
--- Generated by EmmyLua(https:--github.com/EmmyLua)
--- Created by GILLAR.
--- DateTime: 2023/12/15 21:02
---
---@class LoginPanel Login UI页面
local LoginPanel = {}
--- 打开页面
function LoginPanel:Open()
    local panel = UIManager.Instance:GetPanel("LoginPanel");
    if (panel == nil) then
        LoginPanel:Ctor()
    end
end
--- 构造函数
function LoginPanel:Ctor()
    self.panel = UIManager.Instance:ShowPanel("LoginPanel");
    self.transform = self.panel.transform
    self.gameObject = self.panel.gameObject
    self.panel.LoginBtn.onClick:AddListener(function()
        if (self.panel.NameinputField.text ~= "" and self.panel.PassWoldinputField.text ~= "") then
            LoginPanel.Close();
            StartPanel.Open()
        end
    end)
end
--- 关闭页面
function LoginPanel.Close()
    UIManager.Instance:HidePanel("LoginPanel");
end

return LoginPanel