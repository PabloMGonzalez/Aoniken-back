  # Aoniken blog backend

  **Aoniken blog backend** es la solución a un challenge hecho para la compañia **Aoniken** 
  la **API** esta realizada en NetCore 6 C#, utilizando como base de datos MySQL, Dapper como mini ORM para que no haya SQL injections y autenticación via Jason Web Token (JWT)

  ## Prerrequisitos para su funcionamiento
  1. Windows o Linux
  2.  Visual Studio Community (recomendado) o Visual Studio Code
  3. Xampp para poner en funcionamiento la Base de Datos
  4. Git
  5. Chrome, Firefox, etc.

  ## Instalación en Linux

  #### 1º Paso Repositorio
  Clonar el repositorio en la maquina local con la terminal

      git clone  https://github.com/PabloMGonzalez/Aoniken-back.git

  #### 2º Paso Xampp
  1. Insalar Xampp la version 7.4.30 de lo contrario no funcionara del siguiente link [como instalar Xampp en Linux](https://www.neoguias.com/instalar-xampp-linux-mint/ "como instalar Xampp")

  2. Una vez instalado correr los comandos para poder iniciar el servidor apache y mysql:
  <pre> sudo apt install net-tools </pre>
  <pre> sudo /opt/lampp/lampp start</pre>
  3. Si apache tira error al conectarse utilizar el siguiente comando
  <pre>sudo apachectl stop</pre>
  y volver a correr
  <pre> sudo /opt/lampp/lampp start</pre>


  #### 3º Paso Crear BD
  1. Entrar a http://localhost/phpmyadmin/ y crear una Base de Datos llamada **aonikendb**
  2. Una vez hecha la BD importar desde http://localhost/phpmyadmin/,  el archivo **aonikendb.sql** que se encuentra en el repositorio, con el se crean todas las tablas y registros

  ![alt text](https://github.com/PabloMGonzalez/Aoniken-back/blob/main/aoniken1.png?raw=true)
  ![alt text](https://github.com/PabloMGonzalez/Aoniken-back/blob/main/aoniken2.png?raw=true)
  ![alt text](https://github.com/PabloMGonzalez/Aoniken-back/blob/main/aoniken3.png?raw=true)

  3. Luego de la importación exitosa, debemos cambiar el password de la DB por *password* siguiendo la siguiente guia [como cambiar la contraseña desde phpMyAdmin](https://www.mclibre.org/consultar/webapps/lecciones/phpmyadmin-1-soluciones.html)

  #### 4º Paso Dependencias de NetCore 6
  1. Instalar las dependencias de NetCore C# para utilizarlo en Linux siguiendo este link: [como instalar dependencias de netcore para linux](https://learn.microsoft.com/es-es/dotnet/core/install/linux-scripted-manual#scripted-install "como instalar dependencias de netcore para linux")

  #### 5º Paso Ejecutar la API
  1. Ingresar a donde se creo el repositorio y correr el siguiente comando
  <pre>cd Aoniken-back/Aoniken</pre>
  2. Luego para poder compilar y conectar con el servidor correr el siguiente comando
  <pre>dotnet run</pre>

  ![alt text](https://github.com/PabloMGonzalez/Aoniken-back/blob/main/aoniken4.png?raw=true)

  #### 6º Paso solucionar problemas con SSL
  1. Ingresar a la api  desde http://localhost:5020/WeatherForecast y configurar para que no tome los SSL

  #### 7º Paso Instalar Postman para probar la API
  1. Bajar e instalar Postman https://dl.pstmn.io/download/latest/linux64
  2. Loguearse al endpoint http:localhost:5020/user/login con el siguiente cuerpo
  <pre>
  {
        "email":"string",
        "password":"string"
  }
  </pre>
  3. Copiar el result y copiarlo en el Header de los demas End Points para probarlos.


  ## Instalación en Windows

  #### 1º Paso Repositorio
  1. Entrar En Visual Studio Community click en *Clone a Repository*
  2. Copiar el repositorio https://github.com/PabloMGonzalez/Aoniken-back.git en *Repository Location*
  3. Elegir en que carpeta se va a clonar y click en *Clone*

  ![alt text](https://github.com/PabloMGonzalez/Aoniken-back/blob/main/aoniken5.png?raw=true)
  ![alt text](https://github.com/PabloMGonzalez/Aoniken-back/blob/main/aoniken6.png?raw=true)

  #### 2º Paso Xampp
  1. Insalar Xampp la version 7.4.30 de lo contrario no funcionara, del siguiente link [Xampp para Windows](https://sourceforge.net/projects/xampp/files/XAMPP%20Windows/7.4.30/xampp-windows-x64-7.4.30-1-VC15-installer.exe/)
  2. Abrirlo luego click en *start* en apache y mysql para iniciar el servidor y la db

  ![alt text](https://github.com/PabloMGonzalez/Aoniken-back/blob/main/aoniken7.png?raw=true)

  #### 3º Paso Crear BD
  > Ver paso 3 de como crearla en Linux

  #### 4º Paso Ejecutar la API
  1. Con Visual Studio Community ya abierto con el repositorio clonado presionar F5, para compilar y conectar con el servidor
  2. **(opcional)** Swagger UI se abrira mostrando la api, desde aca se pueden hacer pruebas sin la necesidad de Postman

  #### 5º Paso solucionar problemas con SSL
  1. Ingresar a la api desde http://localhost:5020/WeatherForecast y configurar para que no tome los SSL

  #### 6º Paso Instalar Postman para probar la API
  1. Bajar e instalar Postman https://dl.pstmn.io/download/latest/win64
  2. Loguearse al endpoint http:localhost:5020/user/login con el siguiente cuerpo
  <pre>
  {
        "email":"string",
        "password":"string"
  }
  </pre>
  3. Copiar el result y copiarlo en el Header de los demas End Points para probarlos.

  ### END POINTS

  ##### USER

  /user/login
  <pre>
  {
      "email":"string",
      "password":"string"
  }
  </pre>

  /user/register
  <pre>
  {
      "nombre":"string",
      "email":"string",
      "password":"string"
  }
  </pre>

  ##### POSTS

  /post/list_approved_posts
  <pre>
  {
  }
  </pre>

  role 1 editor
  /post/list_pending_approval_posts
  <pre>
  {
  }
  </pre>

  role 2 writer
  /post/list_unapproved_posts
  <pre>
  {
  }
  </pre>

  /post/create_post
  <pre>
  {
    "user_id": int,
    "title": "string",
    "content": "string",
    "submit_date": "2022-11-14T18:59:56.673Z"  
  }
  </pre>

  role 2 writer
  /post/edit_post
  <pre>
  {
    "id":int,
    "title": "string",
    "content": "string",
    "submit_date": "2022-11-14T18:59:56.673Z"  
  }
  </pre>

  role 1 editor
  /post/delete_post
  <pre>
  {
  "id":int
  }
  </pre>

  role 1 editor
  /post/approve_post
  <pre>
  {
  "id":int
  }
  </pre>

  role 1 editor
  /post/reject_post
  <pre>
  {
  "id":int
  }
  </pre>

  /post/select_post
  <pre>
  {
  "id":int
  }
  </pre>

  ##### COMMENTS

  /comment/create_comment
  <pre>
  {
    "user_id":int,
    "post_id":int,
    "content": "string"  
  }
  </pre>

  /comment/list_comments
  <pre>
  {
  }
  </pre>

  ### USER ROLES 
  <pre>role 0 = admin
  role 1 = editor
  role 2 = writer</pre>  

  ### POST PENDING APPROVAL
  <pre>pending_approval 0 = a la espera que se apruebe
  pending_approval 1 = no se aprobo, se puede volver a editar
  pending_approval 2 = se aprobó</pre>  


  #### DIAGRAMA

  ![alt text](https://github.com/PabloMGonzalez/Aoniken-back/blob/main/diagrama.png?raw=true)

  #### SOFTWARE UTILIZADO
  - NetCore 6 C#
  - Dapper
  - Authentication.JwtBearer
  - Newtonsoft.Json
  - MySQL
  - Swagger
  - Visual Studio Community
  - Visual Studio Code
  - phpMyAdmin
  - Postman
  - Chrome
  - DBeaver
  - draw.io


  **Horas requeridas:** 80hs aprox
