import { ErrorMessage, Form, Formik } from 'formik'
import FormTextInput from '../../app/common/form/FormTextInput'
import { Button, Header, Label } from 'semantic-ui-react'
import { observer } from 'mobx-react-lite'
import { useGlobalStore } from '../../app/stores/globalStore'

export default observer(function LoginForm() {
  const { userStore } = useGlobalStore()

  return (
    <Formik
      initialValues={{ email: '', password: '', error: null }}
      onSubmit={(values, { setErrors }) =>
        userStore
          .login(values)
          .catch((_) => setErrors({ error: 'Invalid email or password' }))
      }
    >
      {({ handleSubmit, isSubmitting, errors }) => (
        <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
          <Header
            as="h2"
            content="Login to Reactivities"
            color="teal"
            textAlign="center"
          />
          <FormTextInput name="email" placeholder="Email" />
          <FormTextInput
            name="password"
            placeholder="Password"
            type="password"
          />
          <ErrorMessage
            name="error"
            render={() => (
              <Label
                style={{ marginBottom: 10 }}
                basic
                color="red"
                content={errors.error}
              />
            )}
          />
          <Button
            positive
            loading={isSubmitting}
            content="Login"
            type="submit"
            fluid
          />
        </Form>
      )}
    </Formik>
  )
})
