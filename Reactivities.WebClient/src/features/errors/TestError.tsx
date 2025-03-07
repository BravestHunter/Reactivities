import { Button, Header, Segment } from 'semantic-ui-react'
import axios from 'axios'
import { useState } from 'react'
import ValidationErrors from './ValidationErrors'

export default function TestErrors() {
  const [errors, setErrors] = useState(null)

  function handleNotFound() {
    axios.get('buggy/not-found').catch((err) => console.log(err.response))
  }

  function handleBadRequest() {
    axios.get('/buggy/bad-request').catch((err) => console.log(err.response))
  }

  function handleServerError() {
    axios.get('/buggy/server-error').catch((err) => console.log(err.response))
  }

  function handleUnauthorised() {
    axios.get('/buggy/unauthorised').catch((err) => console.log(err.response))
  }

  function handleBadId() {
    axios.get('/activities/-1').catch((err) => console.log(err.response))
  }

  function handleValidationError() {
    axios.post('/activities', {}).catch((err) => setErrors(err))
  }

  return (
    <>
      <Header as="h1" content="Test Error component" />
      <Segment>
        <Button.Group widths="7">
          <Button onClick={handleNotFound} content="Not Found" basic primary />
          <Button
            onClick={handleBadRequest}
            content="Bad Request"
            basic
            primary
          />
          <Button
            onClick={handleValidationError}
            content="Validation Error"
            basic
            primary
          />
          <Button
            onClick={handleServerError}
            content="Server Error"
            basic
            primary
          />
          <Button
            onClick={handleUnauthorised}
            content="Unauthorised"
            basic
            primary
          />
          <Button onClick={handleBadId} content="Bad Id" basic primary />
        </Button.Group>
      </Segment>
      {errors && <ValidationErrors errors={errors} />}
    </>
  )
}
