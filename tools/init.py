from os import scandir, makedirs
from os.path import abspath, join, exists
from subprocess import run

workdir = abspath(join('..', '.artifacts', 'test'))
if exists(workdir):
    raise FileExistsError('Directory already exists. Remove previous test artifacts before running this script') 

makedirs(workdir)
for proj in scandir(join('..', 'test')):
    csproj = abspath(join(proj.path, f'{proj.name}.csproj'))
    command = ['dotnet', 'pack',  f'{csproj}', '-o', f'{workdir}', '-c', 'Release']
    run(args=command, check=True, timeout=10) 