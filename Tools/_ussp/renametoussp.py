import os
import shutil

root = "../../"
old_name = "имя поддиректория"
new_name = "_USSP"

def renamefilecontent(filepath, old, new):
    """Заменяет old на new внутри файла"""
    try:
        with open(filepath, 'r', encoding='utf-8') as file:
            content = file.read()
        
        if old in content:
            new_content = content.replace(old, new)
            with open(filepath, 'w', encoding='utf-8') as file:
                file.write(new_content)
            print(f"  переименовали содержимое: {filepath}")
            return True
    except:
        pass
    return False
    
def renamefile(root_path, old, new):
    """Заменяет имя файла с old на new"""    
    rename_items = []

    for dirpath, dirnames, filenames in os.walk(root_path, topdown=False):
        for filename in filenames:
            if old in filename:
                old_path = os.path.join(dirpath, filename)
                renamefilecontent(old_path, old, new)
                new_path = os.path.join(dirpath, filename.replace(old, new))
                rename_items.append((old_path, new_path, 'file'))
        
        for dirname in dirnames:
            if old in dirname:
                old_path = os.path.join(dirpath, dirname)
                new_path = os.path.join(dirpath, dirname.replace(old, new))
                rename_items.append((old_path, new_path, 'dir'))
    
    for old_path, new_path, item_type in rename_items:
        if os.path.exists(old_path):
            print(f"переименовали {item_type}: {old_path} в {new_path}")
            shutil.move(old_path, new_path)

renamefile(root, old_name, new_name)