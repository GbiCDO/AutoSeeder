import pyautogui
import time
import sys
import traceback
import os

log_file = open("macro_log.txt", "w")

if len(sys.argv) > 1:
    server_name = sys.argv[1]
else:
    server_name = "Helios"  # fallback/default if nothing passed

print(f"Seeding server: {server_name}")


try:
    # Redirect all output to the log file
    sys.stdout = log_file
    sys.stderr = log_file

    print("Macro starting...")
    def wait_and_click(image, timeout=30, confidence=0.7, grayscale=True):
        start = time.time()
        while True:
            location = pyautogui.locateCenterOnScreen(image, confidence=confidence)
            if location:
                pyautogui.moveTo(location)
                pyautogui.click()
                return True
            if time.time() - start > timeout:
                print(f"Timeout: {image} not found")
                return False
            time.sleep(1)

    # Example usage:
    print("Waiting 120 seconds for game to load...")
    time.sleep(120)

    # pyautogui.screenshot("before_button_click.png")
    # Press ESC (in case of reconnect or popups)
    print("hitting button...")
    pyautogui.press('esc')
    time.sleep(10)

    # pyautogui.screenshot("before_browser_click.png")
    # Click the server browser tab
    print("server browser...")
    wait_and_click('server_browser.png')
    time.sleep(45)

    # Click the search bar
    print("search box...")
    wait_and_click('search_box.png')
    pyautogui.write(server_name)
    pyautogui.press("enter")
    time.sleep(3)

    # Click the first result or "Join" button
    print("join")
    wait_and_click("join_button.png")

except Exception:
    traceback.print_exc()

finally:
    print("Macro complete.")
    log_file.close()
