import { useField } from 'formik'
import { Form, Label } from 'semantic-ui-react'
import DatePicker from 'react-datepicker'

interface CustomDateInputProps {
  name: string
  placeholderText?: string
  showTimeSelect?: boolean
  dateFormat?: string
}

export default function FormDateInput(props: CustomDateInputProps) {
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
