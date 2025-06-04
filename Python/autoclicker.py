import tkinter as tk
from tkinter import ttk, messagebox
import threading
import time

try:
    import pyautogui
except ImportError:
    pyautogui = None


def start_clicking(duration, interval):
    if pyautogui is None:
        messagebox.showerror("Errore", "pyautogui non è installato")
        return
    end_time = time.time() + duration
    while time.time() < end_time:
        pyautogui.click()
        time.sleep(interval)


def on_start(duration_entry, freq_entry, start_button):
    try:
        duration = float(duration_entry.get())
        freq = float(freq_entry.get())
        interval = 1.0 / freq if freq > 0 else 0
        start_button.config(state=tk.DISABLED)
        thread = threading.Thread(target=start_clicking, args=(duration, interval))
        thread.start()
        thread.join()
    except ValueError:
        messagebox.showerror("Errore", "Inserisci valori numerici validi")
    finally:
        start_button.config(state=tk.NORMAL)


def main():
    root = tk.Tk()
    root.title("Auto Clicker")

    ttk.Label(root, text="Durata (secondi):").grid(column=0, row=0, padx=5, pady=5, sticky=tk.W)
    duration_entry = ttk.Entry(root)
    duration_entry.grid(column=1, row=0, padx=5, pady=5)

    ttk.Label(root, text="Frequenza (click al secondo):").grid(column=0, row=1, padx=5, pady=5, sticky=tk.W)
    freq_entry = ttk.Entry(root)
    freq_entry.grid(column=1, row=1, padx=5, pady=5)

    start_button = ttk.Button(root, text="Avvia", command=lambda: on_start(duration_entry, freq_entry, start_button))
    start_button.grid(column=0, row=2, columnspan=2, padx=5, pady=10)

    root.mainloop()


if __name__ == "__main__":
    main()
