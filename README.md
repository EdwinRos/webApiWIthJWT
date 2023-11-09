
# Web API con JWT y Swagger 

En este repositorio veremos como proteger nuestras webs Apis con JWT, 
implementaremos un simple loggin para optener un token en base a un
usuario existente en nuestra base de datos. tambien veremos lo sencillo
que es documentar nuestra api con swagger. (La base de datos esta en la
raiz del repo)




## Temario

 - [Paquetes que se utilizaran](#Paquetes)
 - [Configuracion y desarrollo](#Desarrollo)
 - [Demostracion y nuevo controlador](#Demostracion)
 - [Swagger](#Swagger)
 - [Ejercicio](#Ejercicio) 

# Paquetes

![1](https://i.ibb.co/tDbf6f0/image.png)
![](https://i.ibb.co/jR9nC1S/image.png)
# Desarrollo

#### Configuracion de proyecto

✅  appsettings.json

Dentro de este archivo vamos a agregar nuestra conexión a la base
de datos en el connectionString seguido de la siguiente Configuración.

![appsettings.json](https://i.ibb.co/mGSghgB/appsettings-json.png)

En el apartado de: "Issuer" y "Audience" vamos a colocar local host (Ya que estamos
trabajando en un entorno local) seguido del puerto del mismo
podemos observar el puerto bajo el que trabaja nuestro proyecto si en visual
estudio nos dirigimos a project/projecto->nuestroProyectoPropiedades-> y en la ventana
que se abrirá buscamos las siguientes opciones.

![propperties](https://i.ibb.co/Q9ycNQC/project-Properties.png)

Se nos desplegará una ventana en la cual vamos a poder visualizar lo
siguiente.

![properties2](https://i.ibb.co/bNt2DqL/appsettings-json2.png)

En el apartado de app URL tenemos nuestra ruta sobre la cual se va 
ejecutar nuestra app seguido de su puerto, recordemos que este varía 
de proyecto en proyecto.

✅ Startup.cs

En nuestro archivo de configuracion de la web api vamos a colocar lo
siguiente dentro del metodo ConfigureServices.

![startup1](https://i.ibb.co/BnRKSxq/startup1.png)

Seguido de las configuraciones que ya conocemos para realizar una conexión a la base de datos,
que son las siguientes (si crea una base de datos con un nombre diferente recuerde cambiar su appsettings.json
segun sus requerimientos)

![startup2](https://i.ibb.co/zxFxg5V/startup2.png)

Seguido de esto vamos a agregar una linea en el metodo llamado Configure 

![startup3](https://i.ibb.co/0QGP7PJ/startup3.png)

#### controlador LoginController

Dentro del cotrolador loginController vamos a agregar lo siguiente

✅LoginController.cs

Como propiedades de nuestra clase controladora  y lo inyectamos en nuestro
constructor de la clase controladora justo como lo hemos estado haciendo con el contexto de la base
de datos.

![controller1](https://i.ibb.co/vvjnhQf/Controller.png)

Dentro de esta clase controladora tenemos un método llamado login el cual toma valores de una clase
llamas UserLogin (UserName, Password) podemos observar que dentro de una variable de tipo var, estamos
guardando el resultado de un metodo (creado por nosotros) llamado Authenticate el cual se encarga de ver si las
credenciales proporcionadas pertenecen a un usuario existente en la base de datos. Seguido de eso tenemos
otro método creado por nosotros, este se ejecuta caso que nuestro anterior método Authenticate nos devuelva un valor diferente de nulo
nos generará nuestro token con el cual podremos acceder a los métodos protegidos.

![loggin1](https://i.ibb.co/m4qtnGK/login1.png)

🤓Metodo Authenticate

![loggin2](https://i.ibb.co/xhvtZyy/loggin2.png)

🤓Metodo Generate

![generate](https://i.ibb.co/gvHJrd2/generate.png)

Si observamos este método tenemos que en nuestra var securityKey estamos guardando la key que declaramos
en el appsettings.json, esta se ocupa después para generar una credencial (var credential) importante, note que se está
utilizando el sistema de cifrado HmacSha256 con otros no suele funcionar. Seguido de esto tenemos la var claims
la cual va a guardar diferentes claims las cuales podremos ocupar en un futuro para mayor nivel de seguridad y validaciones.
Por último tenemos nuestra var Token la cual alojara nuestro token bearer dentro de él le colocamos las config las clims 
la expiracion (Para este caso 15 minutos) y el singingCredentials que definimos anteriormente.



---

![diagrama1](https://i.ibb.co/270k8ms/diagrama-consulta.png)

Al momento que realicemos una petición hemos definido
un end-point (método del controlador) el cual no está protegido
es decir, este aceptará peticiones anónimas sin un token. Este
método será el de iniciar sesión en el cual realizamos una consulta 
a la base de datos si se encuentra un usuario que coincida con las
credenciales enviadas, se retornará su información y la web API nos 
devolverá nuestro token.

#### Si al momento de enviar nuestras credenciales estas son correcta:
Obtendremos como respuesta lo siguiente (en su cliente rest de pruebas).

![respuesta1](https://i.ibb.co/BgrkKfr/respuesta-1.png)

En el apartado de response  tenemos el token que se ha generado, el cual
tendremos que incluir en nuestras próximas peticiones a end-points que estén
protegidos con autenticación.



# Demostración

Crearemos un nuevo controlador al cual llamaremos UserController
con el cual vamos a probar lo que hemos realizado.
Primero vamos a declarar un método de acceso público sin autenticación por token.

![public](https://i.ibb.co/5Y7tPJw/public1.png)

Si nosotros probamos esta ruta en nuestro cliente rest no tendremos ningún problema
momento de consultarlo.

![prest](https://i.ibb.co/5YWDKyj/public-Res.png)

Procederemos a crear un siguiente método el cual llamaremos adminUser el cual se deberá
de ver de la siguiente manera

![admin](https://i.ibb.co/2yhVYKk/image.png)

El metodo getCurrentUserInfo() es generado por nosotros y va de la siguiente manera

![getUser](https://i.ibb.co/f4mz04J/image.png)
--
#### Probando nuestro end-point admins el cual esta protegido sin enviar el token

![rest2](https://i.ibb.co/stBQnKW/image.png)

Como podemos observar el metodo esta esperando el token de validacion de inicio de sesion.
Como este no esta siendo proporcionado no esta devolviendo un error 401 el cual nos dice 
que no estamos autorizados para acceder.

#### Realizando misma peticion proporcionando token (recuerde primero 'iniciar sesion' para obtener su token


1) PostMan

 ![postman](https://i.ibb.co/CP9j4gF/image.png)

 En Type seleccionamos Bearer Token ene l apartado de token ingresamos el token que se nos 
 asigno

 2)Insomnia
 ![Insomnia1](https://i.ibb.co/SKyKdLM/image.png)

 ![Insomnia2](https://i.ibb.co/0DCFvLn/image.png)   


De esta forma nosotros podemos agregar autenticación a nuestras aplicaciones

# Swagger

Para documentar con swagger esta misma API haremos lo siguiente

#### vamos a agreagar este paquete

![paq2](https://i.ibb.co/DY21XS6/image.png)


Luego vamos a agregar la siguiente configuracion a nuestro startup.cs

Dentro del metodo ConfigureServices vamos a agreagar lo siguiente

![](https://i.ibb.co/3RVVnyv/image.png)

Luego vamos a agreagar lo siguiente en el metodo Configure

![](https://i.ibb.co/RYd97mY/image.png)

Una vez realizado esto vamos a ejecutar nuestra aplicacion y vamos a digitar \
localhost:{reemplazar por el puerto de su app}/swagger/index.html#

![](https://i.ibb.co/DRpg6Wm/image.png)

en la parte baja podremos observar las rutas iniales de nuestros controladores
arriba tenemos un boton el cual nos va a permitir agregar nuestro token para 
poder acceder a los end-points protegidos de nuestra app.

Ejemplo:

![](https://i.ibb.co/nc8f1VK/image.png)

el token lo colocaremos en el boton Authorize de la parte superior

![](https://i.ibb.co/Tqh0mbc/image.png)

Al inicio del input colocaremos Bearer seguido de un espacio y nuestro token
daremos en Authorize y cerraremos con el boton de close seguido de esto vamos
a consultar nuestro metodo protegido de admins.

![](https://i.ibb.co/QCDTvc1/image.png)

Como podemos observar no tenemos ningun codigo 401 de no autorizado nos devuelve nuestra informacion del usuario
con el que nos hemos loggeado en la app. asi de simple asi de sencillo sin mas configuracion el lo hace automaticamente
🧙‍♂️🧙‍♂️🧙‍♂️

---
# Ejercicio
En este repositorio tenemos solo un metodo de login y uno de obtener la informacion del usuario que ha iniciado sesion, completar este repositorio para poder 
ingresar mas usuarios con roles diferentes (revisar base de datos) solo los usuarios con rol administrator podran ingresar y eliminar diferentes usuarios .

