import "./Page.css"

export default function Page() {
    return (
        <div className="mainPage">

            <div>
                <h1>Rotas do Usuário</h1>
                <div class="row row-cols-2">
                    <div class="col">
                        <h3>CreateStudent</h3>
                        <p>/student - POST</p>
                    </div>
                    <div class="col">
                        <h3>Login</h3>
                        <p>/login - POST</p>
                    </div>
                    <div class="col">
                        <h3>UpdateStudent</h3>
                        <p>/student/id - PUT</p>
                    </div>
                    <div class="col">
                        <h3>DeleteStudent</h3>
                        <p>/student/id - DELETE</p>
                    </div>
                </div>
            </div>

            <div>
                <h1>Rotas do Post</h1>
                <div class="row row-cols-2">
                    <div class="col">

                    </div>
                    <div class="col">

                    </div>
                    <div class="col">

                    </div>
                    <div class="col">

                    </div>
                </div>
            </div>

        </div>
    )
}
