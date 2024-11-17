import { ErrorMessage, Form, Formik } from 'formik'
import FormTextInput from '../../app/common/form/FormTextInput'
import { Button, Header } from 'semantic-ui-react'
import { observer } from 'mobx-react-lite'
import * as Yup from 'yup'
import ValidationErrors from '../errors/ValidationErrors'
import { useGlobalStore } from '../../app/stores/globalStore'

export default observer(function RegisterForm() {
  const { userStore } = useGlobalStore()

  return (
    <Formik
      initialValues={{
        username: '',
        displayName: '',
        email: '',
        password: '',
        error: null,
      }}
      onSubmit={(values, { setErrors }) =>
        userStore.register(values).catch((error) => setErrors({ error }))
      }
      validationSchema={Yup.object({
        username: Yup.string().required(),
        displayName: Yup.string().required(),
        email: Yup.string().required(),
        password: Yup.string().required(),
      })}
    >
      {({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
        <Form
          className="ui form error"
          onSubmit={handleSubmit}
          autoComplete="off"
        >
          <Header
            as="h2"
            content="Sign up to Reactivities"
            color="teal"
            textAlign="center"
          />
          <FormTextInput name="username" placeholder="Username" />
          <FormTextInput name="displayName" placeholder="Display Name" />
          <FormTextInput name="email" placeholder="Email" />
          <FormTextInput
            name="password"
            placeholder="Password"
            type="password"
          />
          <ErrorMessage
            name="error"
            render={() => <ValidationErrors errors={errors.error} />}
          />
          <Button
            disabled={!isValid || !dirty || isSubmitting}
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
