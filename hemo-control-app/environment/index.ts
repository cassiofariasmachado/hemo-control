type Environment = {
    apiUrl: string
}

const environment: Environment = {
    apiUrl: process.env.API_URL || ''
};

export default environment;