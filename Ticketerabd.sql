CREATE TYPE ticket_status AS ENUM ('abierto', 'en_proceso', 'cerrado');


CREATE TABLE roles (
                       role_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                       role_name VARCHAR(50) NOT NULL UNIQUE
);


CREATE TABLE users (
                       user_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                       username VARCHAR(100) NOT NULL UNIQUE,
                       password_hash VARCHAR(255) NOT NULL,
                       email VARCHAR(150) UNIQUE,
                       created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now()
);


CREATE TABLE user_roles (
                            user_id UUID NOT NULL,
                            role_id UUID NOT NULL,
                            assigned_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),
                            PRIMARY KEY (user_id, role_id),
                            CONSTRAINT fk_user_roles_user FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
                            CONSTRAINT fk_user_roles_role FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE CASCADE
);

CREATE TABLE tickets (
                         ticket_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                         user_id UUID NOT NULL,
                         title VARCHAR(255) NOT NULL,
                         description TEXT,
                         status ticket_status NOT NULL DEFAULT 'abierto',
                         created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),
                         closed_at TIMESTAMP WITHOUT TIME ZONE NULL,
                         CONSTRAINT fk_tickets_user FOREIGN KEY (user_id) REFERENCES users(user_id)
);


CREATE TABLE responses (
                           response_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                           ticket_id UUID NOT NULL,
                           responder_id UUID NOT NULL,
                           message TEXT NOT NULL,
                           created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),
                           CONSTRAINT fk_responses_ticket FOREIGN KEY (ticket_id) REFERENCES tickets(ticket_id) ON DELETE CASCADE,
                           CONSTRAINT fk_responses_responder FOREIGN KEY (responder_id) REFERENCES users(user_id)
);


CREATE INDEX idx_user_roles_role_id ON user_roles(role_id);
CREATE INDEX idx_tickets_user_id ON tickets(user_id);
CREATE INDEX idx_tickets_status ON tickets(status);
CREATE INDEX idx_responses_ticket_id ON responses(ticket_id);
CREATE INDEX idx_responses_responder_id ON responses(responder_id);