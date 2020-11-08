function love.load()
  if DEBUG_MODE then
    if arg[#arg] == "-debug" then 
      require("mobdebug").start() 
    end
    io.stdout:setvbuf("no")
  end
  
  math.randomseed(os.time())

  -- Set up window
  love.window.setMode(800, 450)
  love.window.setTitle("Shaders sandbox")
  
  shader = love.graphics.newShader("shaders/2D_gradient.fs")
  shader:send("resolution", { love.graphics.getWidth(), love.graphics.getHeight() })
  
  love.keyboard.keysPressed = {}
  love.mouse.buttonPressed = {}
  love.mouse.buttonReleased = {}
end

function love.update(dt)
  -- exit if esc is pressed
  if love.keyboard.keysPressed['escape'] then
    love.event.quit()
  end
  
  love.keyboard.keysPressed = {}
  love.mouse.buttonPressed = {}
  love.mouse.buttonReleased = {}
end

function love.resize(w, h)
  push:resize(w, h)
end
  
-- Callback that processes key strokes just once
-- Does not account for keys being held down
function love.keypressed(key)
  love.keyboard.keysPressed[key] = true
end

function love.mousepressed(x, y, button)
  love.mouse.buttonPressed[button] = true
end

function love.mousereleased(x, y, button)
  love.mouse.buttonReleased[button] = true
end

function love.draw()
  love.graphics.setShader(shader)
  love.graphics.rectangle('fill', 0, 0, love.graphics.getWidth(), love.graphics.getHeight())
  love.graphics.setShader()

  local fps = love.timer.getFPS()
  if fps >= 60 then
    love.graphics.setColor(0, 1, 0)
  else
    love.graphics.setColor(1, 0, 0)
  end
  love.graphics.setColor(1, 1, 1)
  love.graphics.printf("FPS: " .. fps, 5, 5, 1000, 'left')
  love.graphics.printf("Width: " .. love.graphics.getWidth(), 5, 17, love.graphics.getWidth(), 'left')
  love.graphics.printf("Height: " .. love.graphics.getHeight(), 5, 29, love.graphics.getWidth(), 'left')
end