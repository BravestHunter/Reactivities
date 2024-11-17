import { useField } from 'formik'
import { Form, Label } from 'semantic-ui-react'
import DatePicker from 'react-datepicker'
import { DatePickerProps } from 'react-datepicker'

export type CustomDateInputProps = DatePickerProps & {
  name: string
}

export default function CustomDateInput(props: CustomDateInputProps) {
  const [field, meta, helpers] = useField(props.name)

  const handleChange = (date: Date | null) => {
    helpers.setValue(date)
  }

  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <DatePicker
        {...field}
        {...props}
        selected={(field.value && new Date(field.value)) || null}
        onChange={handleChange}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  )
}
