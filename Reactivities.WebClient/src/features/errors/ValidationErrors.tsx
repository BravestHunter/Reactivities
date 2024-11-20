import { Message } from "semantic-ui-react";

interface Props {
  errors: any;
}

export default function ValidationErrors(props: Props) {
  const { errors } = props;

  return (
    <Message error>
      <Message.List>
        {errors.map((err: string, i: any) => (
          <Message.Item key={i}>{err}</Message.Item>
        ))}
      </Message.List>
    </Message>
  );
}
